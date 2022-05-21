using SkyWing.NBT.Serializer;

namespace SkyWing.NBT.Tag; 

public class ByteTag : ImmutableTag {

	private readonly sbyte _value;
	
	public ByteTag(sbyte value) {
		_value = value;
	}
	
	public override object GetValue() {
		return _value;
	}

	public override int GetTagType() {
		return NBT.TAG_Byte;
	}

	public override void Write(NbtStreamWriter writer) {
		writer.WriteByte(_value);
	}

	public static ByteTag Read(NbtStreamReader reader) {
		return new ByteTag(reader.ReadSignedByte());
	}

	public override string GetTypeName() {
		return "Byte";
	}

	public override string StringifyValue(int indentation) {
		return Convert.ToString(_value);
	}
}