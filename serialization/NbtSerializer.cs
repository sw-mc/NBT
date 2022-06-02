using System.Text;
using SkyWing.Binary;
using SkyWing.NBT.Tag;
using SkyWing.NBT.Utils;

namespace SkyWing.NBT.Serialization;

public abstract class BaseNbtSerializer : NbtStreamReader, NbtStreamWriter {
	
	protected BinaryStream Buffer;
	
	public BaseNbtSerializer() {
		Buffer = new BinaryStream(new MemoryStream());
	}

	private TreeRoot ReadRoot(int maxDepth) {
		var type = ReadByte();
		if (type == (int) TagType.End)
			throw new NbtDataException("Found TAG_End at the start of buffer");
		
		return new TreeRoot(NBT.CreateTag((TagType) type, this, new ReaderTracker(maxDepth)), ReadString());
	}

	public TreeRoot Read(MemoryStream buffer, int maxDepth) {
		Buffer = new BinaryStream(buffer);

		try {
			return ReadRoot(maxDepth);
		}
		catch (BinaryDataException e) {
			throw new NbtDataException("Error reading NBT data: " + e.Message);
		}
	}

	private void WriteRoot(TreeRoot root) {
		WriteByte(root.Root.GetTagType());
		WriteString(root.Name);
		root.Root.Write(this);
	}
	
	public BinaryStream Write(TreeRoot root) {
		Buffer = new BinaryStream(new MemoryStream());
		
		try {
			WriteRoot(root);
		}
		catch (BinaryDataException e) {
			throw new NbtDataException("Error writing NBT data: " + e.Message);
		}
		
		return Buffer;
	}

	public byte ReadByte() {
		return Buffer.ReadByte();
	}

	public sbyte ReadSignedByte() {
		return BinaryStream.SignByte(ReadByte());
	}

	public abstract short ReadShort();

	public abstract short ReadSignedShort();

	public abstract int ReadInt();
	
	public abstract long ReadLong();

	public abstract float ReadFloat();

	public abstract double ReadDouble();

	public byte[] ReadByteArray() {
		return Buffer.ReadBytes(Buffer.ReadInt());
	}

	public string ReadString() {
		return Encoding.UTF8.GetString(Buffer.ReadBytes(ReadShort()));
	}

	public abstract int[] ReadIntArray();

	public abstract long[] ReadLongArray();

	public void WriteByte(byte value) {
		Buffer.WriteByte(value);
	}

	public abstract void WriteShort(short value);
	
	public abstract void WriteInt(int value);
	
	public abstract void WriteLong(long value);

	public abstract void WriteFloat(float value);

	public abstract void WriteDouble(double value);

	public void WriteByteArray(byte[] value) {
		WriteInt(value.Length);
		Buffer.WriteBytes(value);
	}

	public void WriteString(string value) {
		WriteShort((short) Encoding.UTF8.GetByteCount(value));
		Buffer.WriteBytes(Encoding.UTF8.GetBytes(value));
	}

	public abstract void WriteIntArray(int[] value);

	public abstract void WriteLongArray(long[] value);
}

public class BigEndianNbtSerializer : BaseNbtSerializer {

	public override short ReadShort() {
		return Buffer.ReadShort();
	}

	public override short ReadSignedShort() {
		return Buffer.ReadSignedShort();
	}

	public override int ReadInt() {
		return Buffer.ReadInt();
	}

	public override long ReadLong() {
		return Buffer.ReadLong();
	}

	public override float ReadFloat() {
		return Buffer.ReadFloat();
	}

	public override double ReadDouble() {
		return Buffer.ReadDouble();
	}

	public override int[] ReadIntArray() {
		var length = ReadInt();
		var array = new int[length];
		for (var i = 0; i < length; i++)
			array[i] = ReadInt();
		return array;
	}

	public override long[] ReadLongArray() {
		var length = ReadInt();
		var array = new long[length];
		for (var i = 0; i < length; i++)
			array[i] = ReadLong();
		return array;
	}

	public override void WriteShort(short value) {
		Buffer.WriteShort(value);
	}

	public override void WriteInt(int value) {
		Buffer.WriteInt(value);
	}

	public override void WriteLong(long value) {
		Buffer.WriteLong(value);
	}

	public override void WriteFloat(float value) {
		Buffer.WriteFloat(value);
	}

	public override void WriteDouble(double value) {
		Buffer.WriteDouble(value);
	}

	public override void WriteIntArray(int[] value) {
		WriteInt(value.Length);
		foreach (var i in value)
			WriteInt(i);
	}

	public override void WriteLongArray(long[] value) {
		WriteInt(value.Length);
		foreach (var i in value)
			WriteLong(i);
	}
	
}

public class LittleEndianNbtSerializer : BaseNbtSerializer {
	
	public override short ReadShort() {
		return Buffer.ReadLShort();
	}

	public override short ReadSignedShort() {
		return Buffer.ReadSignedLShort();
	}

	public override int ReadInt() {
		return Buffer.ReadLInt();
	}

	public override long ReadLong() {
		return Buffer.ReadLLong();
	}

	public override float ReadFloat() {
		return Buffer.ReadLFloat();
	}

	public override double ReadDouble() {
		return Buffer.ReadLDouble();
	}

	public override int[] ReadIntArray() {
		var length = ReadInt();
		var array = new int[length];
		for (var i = 0; i < length; i++)
			array[i] = ReadInt();
		return array;
	}

	public override long[] ReadLongArray() {
		var length = ReadInt();
		var array = new long[length];
		for (var i = 0; i < length; i++)
			array[i] = ReadLong();
		return array;
	}

	public override void WriteShort(short value) {
		Buffer.WriteLShort(value);
	}

	public override void WriteInt(int value) {
		Buffer.WriteLInt(value);
	}

	public override void WriteLong(long value) {
		Buffer.WriteLLong(value);
	}

	public override void WriteFloat(float value) {
		Buffer.WriteLFloat(value);
	}

	public override void WriteDouble(double value) {
		Buffer.WriteLDouble(value);
	}

	public override void WriteIntArray(int[] value) {
		WriteInt(value.Length);
		foreach (var i in value)
			WriteInt(i);
	}

	public override void WriteLongArray(long[] value) {
		WriteInt(value.Length);
		foreach (var i in value)
			WriteLong(i);
	}
	
}