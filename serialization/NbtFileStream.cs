using System.IO.Compression;
using SkyWing.Binary;
using SkyWing.NBT.Serialization.Compression;
using SkyWing.NBT.Tag;
using SkyWing.NBT.Utils;

namespace SkyWing.NBT.Serialization;

public class NbtFileStream : BaseBinaryStream {

	// Size of buffers that are used to avoid frequent reads from / writes to compressed streams
	const int WriteBufferSize = 8 * 1024;

	// Size of buffers used for reading to/from files
	private const int FileStreamBufferSize = 64 * 1024;

	public NbtCompression Compression { get; private set; }
	public TreeRoot? RootTag { get; private set; }

	public static bool BigEndianByDefault { get; }

	public bool BigEndian { get; set; }

	public static int DefaultBufferSize {
		get => _defaultBufferSize;
		set {
			if (value < 0)
				throw new ArgumentException("Buffer size cannot be negative.");
			_defaultBufferSize = value;
		}
	}

	static int _defaultBufferSize = 8 * 1024;

	public int BufferSize {
		get => _bufferSize;
		set {
			if (value < 0)
				throw new ArgumentException("Buffer size cannot be negative.");
			_bufferSize = value;
		}
	}

	private int _bufferSize;

	static NbtFileStream() {
		BigEndianByDefault = true;
	}

	public NbtFileStream(FileStream stream, NbtCompression compression = NbtCompression.AutoDetect,
		TreeRoot? rootTag = null, string tagName = "") : base(stream) {
		Stream = stream;
		Compression = compression;
		BigEndian = BigEndianByDefault;
		//BufferSize = _defaultBufferSize;
		RootTag = rootTag ?? new TreeRoot(new CompoundTag(), tagName);
	}

	public static NbtFileStream LoadFromFile(string fileName, NbtCompression compression = NbtCompression.AutoDetect) {
		using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read,
			FileStreamBufferSize, FileOptions.SequentialScan);

		var stream = new NbtFileStream(fileStream, compression);
		stream.Reader = new NbtBinaryReader(fileStream, stream.BigEndian);
		stream.LoadFromStream(compression);
		return stream;
	}

	public void LoadFromStream(NbtCompression compression) {
		Compression = compression == NbtCompression.AutoDetect ? DetectCompression(Stream) : compression;

		switch (compression) {
			case NbtCompression.None:
				LoadFromStreamInternal(Stream);
				break;
			case NbtCompression.GZip: {
				using var decStream = new GZipStream(Stream, CompressionMode.Decompress, true);
				if (_bufferSize > 0) {
					LoadFromStreamInternal(new BufferedStream(Stream, _bufferSize));
				}
				else {
					LoadFromStreamInternal(decStream);
				}

				break;
			}
			case NbtCompression.ZLib when Stream.ReadByte() != 0x78:
				throw new InvalidDataException("Zlib stream must have zlib header.");
			case NbtCompression.ZLib: {
				Stream.ReadByte();
				using var decStream = new DeflateStream(Stream, CompressionMode.Decompress, true);
				if (_bufferSize > 0) {
					LoadFromStreamInternal(new BufferedStream(decStream, _bufferSize));
				}
				else {
					LoadFromStreamInternal(decStream);
				}

				break;
			}
			default:
				throw new ArgumentOutOfRangeException(nameof(compression));
		}
	}

	private void LoadFromStreamInternal(Stream readStream) {
		var firstByte = readStream.ReadByte();
		if (firstByte < 0) {
			throw new EndOfStreamException();
		}

		if (firstByte != (byte)TagType.Compound) {
			throw new NbtDataException("Given NBT stream does not start with a TAG_Compound");
		}

		Reader = new NbtBinaryReader(readStream, BigEndian);
		var name = Reader.ReadString();
		RootTag = new TreeRoot(NBT.CreateTag(TagType.Compound, Reader), name);
	}

	static NbtCompression DetectCompression(Stream stream) {
		if (!stream.CanSeek) {
			throw new NotSupportedException("Cannot auto-detect compression on a stream that's not seekable.");
		}

		var firstByte = stream.ReadByte();
		var compression = firstByte switch {
			-1 => throw new EndOfStreamException(),
			(byte)TagType.Compound => // 0x0A
				NbtCompression.None,
			0x1F =>
				// GZip magic number
				NbtCompression.GZip,
			0x78 =>
				// ZLib header
				NbtCompression.ZLib,
			_ => throw new InvalidDataException("Could not auto-detect compression format.")
		};
		stream.Seek(-1, SeekOrigin.Current);
		return compression;
	}

	public static NbtFileStream SaveToFile(string fileName, NbtCompression compression) {
		using var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None,
			FileStreamBufferSize, FileOptions.SequentialScan);
		var stream = new NbtFileStream(fileStream, compression);
		stream.SaveToStream(compression);
		return stream;
	}

	public void SaveToStream(NbtCompression compression) {
		switch (compression) {
			case NbtCompression.AutoDetect:
				throw new ArgumentException("AutoDetect is not a valid NbtCompression value for saving.");
			case NbtCompression.ZLib:
			case NbtCompression.GZip:
			case NbtCompression.None:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(compression));
		}

		switch (compression) {
			case NbtCompression.ZLib:
				Stream.WriteByte(0x78);
				Stream.WriteByte(0x01);
				int checksum;
				using (var compressStream = new Compression.ZLibStream(Stream, CompressionMode.Compress, true)) {
					var bufferedStream = new BufferedStream(compressStream, WriteBufferSize);
					RootTag.Root.Write(new NbtBinaryWriter(bufferedStream, BigEndian));
					bufferedStream.Flush();
					checksum = compressStream.Checksum;
				}

				byte[] checksumBytes = BitConverter.GetBytes(checksum);
				if (BitConverter.IsLittleEndian) {
					// Adler32 checksum is big-endian
					Array.Reverse(checksumBytes);
				}

				Stream.Write(checksumBytes, 0, checksumBytes.Length);
				break;

			case NbtCompression.GZip:
				using (var compressStream = new GZipStream(Stream, CompressionMode.Compress, true)) {
					// use a buffered stream to avoid GZipping in small increments (which has a lot of overhead)
					var bufferedStream = new BufferedStream(compressStream, WriteBufferSize);
					RootTag.Root.Write(new NbtBinaryWriter(bufferedStream, BigEndian));
					bufferedStream.Flush();
				}

				break;
			
			case NbtCompression.None:
				Writer = new NbtBinaryWriter(Stream, BigEndian);
				Writer.WriteByte((byte)TagType.Compound);
				Writer.WriteString(RootTag.Name);
				RootTag.Root.Write(Writer);
				break;
			
			default:
				throw new ArgumentOutOfRangeException(nameof(compression));

			// Can't be AutoDetect or unknown: parameter is already validated
		}
	}
}