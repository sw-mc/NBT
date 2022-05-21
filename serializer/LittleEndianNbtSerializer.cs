using System.Buffers.Binary;
using SkyWing.Binary;

namespace SkyWing.NBT.Serializer; 

public class LittleEndianNbtSerializer : BaseNbtSerializer {
	public LittleEndianNbtSerializer(BinaryStream stream) : base(stream) { }
	
	public override ushort ReadShort() {
		return BinaryPrimitives.ReverseEndianness(stream.GetShort());
	}

	public override short ReadSignedShort() {
		return BinaryPrimitives.ReverseEndianness(stream.GetSignedShort());
	}

	public override int ReadInt() {
		return BinaryPrimitives.ReverseEndianness(stream.GetSignedInt());
	}

	public override long ReadLong() {
		return BinaryPrimitives.ReverseEndianness(stream.GetSignedLong());
	}

	public override float ReadFloat() {
		return stream.GetFloat();
	}

	public override double ReadDouble() {
		return stream.GetDouble();
	}

	public override int[] ReadIntArray() {
		int length = ReadInt();
		int[] array = new int[length];
		for (int i = 0; i < length; i++) {
			array[i] = Convert.ToInt32(BinaryPrimitives.ReverseEndianness(stream.GetInt()));
		}
		return array;
	}

	public override void WriteShort(short value) {
		stream.PutSignedShort(value);
	}

	public override void WriteShort(ushort value) {
		stream.PutShort(value);
	}

	public override void WriteInt(int value) {
		stream.PutSignedInt(BinaryPrimitives.ReverseEndianness(value));
	}

	public override void WriteInt(uint value) {
		stream.PutInt(BinaryPrimitives.ReverseEndianness(value));
	}

	public override void WriteLong(long value) {
		stream.PutSignedLong(BinaryPrimitives.ReverseEndianness(value));
	}

	public override void WriteLong(ulong value) {
		stream.PutLong(BinaryPrimitives.ReverseEndianness(value));
	}

	public override void WriteFloat(float value) {
		stream.PutFloat(value);
	}

	public override void WriteDouble(double value) {
		stream.PutDouble(value);
	}

	public override void WriteIntArray(int[] value) {
		WriteInt((uint)value.Length);
		foreach (var t in value) {
			WriteInt(BinaryPrimitives.ReverseEndianness(t));
		}
	}
}