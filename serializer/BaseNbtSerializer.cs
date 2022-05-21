using System.Text;
using DefaultNamespace;
using SkyWing.Binary;
using SkyWing.NBT.Utils;

namespace SkyWing.NBT.Serializer;

public abstract class BaseNbtSerializer : NbtStreamReader, NbtStreamWriter {
	
	protected BinaryStream stream;
	
	protected BaseNbtSerializer(BinaryStream stream) {
		this.stream = stream;
	}

	private TreeRoot ReadRoot() {
		var type = ReadByte();
		if (type == NBT.TAG_End)
			throw new NbtDataException("Found TAG_End at the start of buffer");
		
		var rootName = ReadString();
		return new TreeRoot(NBT.CreateTag(type, this), rootName);
	}

	public TreeRoot Read(byte[] buffer, ref int offset) {
		stream = new BinaryStream(buffer.Length, buffer, offset);

		try {
			var data = ReadRoot();
			offset = stream.GetOffset();
			return data;
		}
		catch (BinaryDataException e) {
			throw new NbtDataException(e.Message);
		}
	}
	
	private void WriteRoot(TreeRoot root) {
		WriteByte((byte) root.Root.GetTagType());
		WriteString(root.Name);
		root.Root.Write(this);
	}
	
	public byte[] Write(TreeRoot root) {
		stream = new BinaryStream(root.Name.Length + 1);
		
		WriteRoot(root);
		
		return stream.GetBuffer();
	}

	public byte[] WriteMultiple(TreeRoot[] data) {
		stream = new BinaryStream(data.Length);
		foreach (var root in data)
			WriteRoot(root);
		
		return stream.GetBuffer();
	}
	
	public byte ReadByte() {
		return stream.GetByte();
	}

	public sbyte ReadSignedByte() {
		return Convert.ToSByte(stream.GetByte());
	}

	public abstract ushort ReadShort();

	public abstract short ReadSignedShort();

	public abstract int ReadInt();

	public abstract long ReadLong();

	public abstract float ReadFloat();

	public abstract double ReadDouble();

	public string ReadString() {
		return Encoding.UTF8.GetString(stream.Get(stream.GetShort()));
	}

	public byte[] ReadByteArray() {
		return stream.Get(Convert.ToInt32(stream.GetInt()));
	}

	public abstract int[] ReadIntArray();

	public void WriteByte(byte value) {
		stream.PutByte(value);
	}

	public void WriteByte(sbyte value) {
		stream.PutSByte(value);
	}

	public abstract void WriteShort(short value);

	public abstract void WriteShort(ushort value);

	public abstract void WriteInt(int value);

	public abstract void WriteInt(uint value);

	public abstract void WriteLong(long value);

	public abstract void WriteLong(ulong value);

	public abstract void WriteFloat(float value);

	public abstract void WriteDouble(double value);

	public void WriteString(string value) {
		WriteShort(Convert.ToInt16(value.Length));
		stream.Put(Encoding.UTF8.GetBytes(value));
	}

	public void WriteByteArray(byte[] value) {
		WriteInt(value.Length);
		stream.Put(value);
	}

	public abstract void WriteIntArray(int[] value);
}