using System.Text;
using SkyWing.Binary;

namespace SkyWing.NBT.Serializer; 

public class BigEndianNbtSerializer : BaseNbtSerializer {
	public BigEndianNbtSerializer(BinaryStream stream) : base(stream) { }
	
	public override ushort ReadShort() {
		return stream.GetShort();
	}

	public override short ReadSignedShort() {
		return stream.GetSignedShort();
	}

	public override int ReadInt() {
		return stream.GetSignedInt();
	}

	public override long ReadLong() {
		return stream.GetSignedLong();
	}

	public override float ReadFloat() {
		return stream.GetFloat();
	}

	public override double ReadDouble() {
		return stream.GetDouble();
	}

	public override int[] ReadIntArray() {
		var length = ReadInt();
		var ints = new int[length];
		for (int i = 0; i < length; i++) {
			ints[i] = Convert.ToInt32(ReadInt());
		}

		return ints;
	}

	public override void WriteShort(short value) {
		stream.PutSignedShort(value);
	}

	public override void WriteShort(ushort value) {
		stream.PutShort(value);
	}

	public override void WriteInt(int value) {
		stream.PutSignedInt(value);
	}

	public override void WriteInt(uint value) {
		stream.PutInt(value);
	}

	public override void WriteLong(long value) {
		stream.PutSignedLong(value);
	}

	public override void WriteLong(ulong value) {
		stream.PutLong(value);
	}

	public override void WriteFloat(float value) {
		stream.PutFloat(value);
	}

	public override void WriteDouble(double value) {
		stream.PutDouble(value);
	}

	public override void WriteIntArray(int[] value) {
		WriteInt(value.Length);
		foreach (var t in value)
			WriteInt(t);
	}
}