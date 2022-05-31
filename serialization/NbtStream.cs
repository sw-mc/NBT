using SkyWing.NBT.Utils;

namespace SkyWing.NBT.Serialization;

public interface NbtStreamReader {

	public byte ReadByte();
	
	public sbyte ReadSignedByte();
	
	public short ReadShort();
	
	public short ReadSignedShort();

	public int ReadInt();
	
	public long ReadLong();
	
	public float ReadFloat();

	public double ReadDouble();
	
	public byte[] ReadByteArray();
	
	public string ReadString();
	
	public int[] ReadIntArray();

	public long[] ReadLongArray();
	
}

public interface NbtStreamWriter {
	
	public void WriteByte(byte value);
	
	public void WriteShort(short value);
	
	public void WriteInt(int value);
	
	public void WriteLong(long value);
	
	public void WriteFloat(float value);
	
	public void WriteDouble(double value);
	
	public void WriteByteArray(byte[] value);
	
	public void WriteString(string value);
	
	public void WriteIntArray(int[] value);
	
	public void WriteLongArray(long[] value);
	
}

public class ReaderTracker {

	public int MaxDepth {
		get {
			return _maxDepth;
		}
		init {
			if (value < 0) {
				throw new ArgumentOutOfRangeException(nameof(value), "MaxDepth cannot be negative");
			}
			_maxDepth = value;
		}
	}

	private readonly int _maxDepth;

	public int Depth { get; private set; }
	
	public ReaderTracker(int maxDepth) {
		MaxDepth = maxDepth;
		Depth = 0;
	}

	public void ProtectDepth(Action<bool> execute) {
		if (MaxDepth > 0 && ++Depth > MaxDepth) {
			throw new NbtDataException("Nested level too deep: reached a max depth of " + MaxDepth + " tags");
		}
		try {
			execute(true);
		}
		finally {
			--Depth;
		}
	}
}