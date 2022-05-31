using SkyWing.NBT.Serialization;

namespace SkyWing.NBT.Tag; 

public class ByteTag : ImmutableTag {

	private readonly sbyte _value;
	
	public ByteTag(sbyte value) {
		_value = value;
	}
	
	public override object GetValue() {
		return _value;
	}

	public override byte GetTagType() {
		return (byte)TagType.Byte;
	}

	public override void Write(NbtStreamWriter writer) {
		writer.WriteByte(_value);
	}

	public static ByteTag Read(NbtStreamReader reader) {
		return new ByteTag(reader.ReadByte());
	}

	public override string GetTypeName() {
		return "Byte";
	}

	public override string StringifyValue(int indentation) {
		return Convert.ToString(_value);
	}
}