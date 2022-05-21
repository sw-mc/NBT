namespace SkyWing.NBT.Serializer;

public interface NbtStreamReader {
	
	public byte ReadByte();

	public sbyte ReadSignedByte();
	
	public ushort ReadShort();
	
	public short ReadSignedShort();
	
	public int ReadInt();
	
	public long ReadLong();
	
	public float ReadFloat();
	
	public double ReadDouble();
	
	public string ReadString();
	
	public byte[] ReadByteArray();
	
	public int[] ReadIntArray();
}

public interface NbtStreamWriter {
	
	public void WriteByte(byte value);

	public void WriteByte(sbyte value);
	
	public void WriteShort(short value);
	
	public void WriteShort(ushort value);

	public void WriteInt(int value);
	
	public void WriteInt(uint value);
	
	public void WriteLong(long value);
	
	public void WriteLong(ulong value);
	
	public void WriteFloat(float value);
	
	public void WriteDouble(double value);
	
	public void WriteString(string value);
	
	public void WriteByteArray(byte[] value);
	
	public void WriteIntArray(int[] value);
}