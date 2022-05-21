using SkyWing.NBT.Serializer;

namespace SkyWing.NBT.Tag;

public class IntTag : ImmutableTag {

	private readonly int _value;
	
	public IntTag(int value) {
		_value = value;
	}
	
	public override object GetValue() {
		return _value;
	}

	public override int GetTagType() {
		return NBT.TAG_Int;
	}

	public override void Write(NbtStreamWriter writer) {
		writer.WriteInt(_value);
	}
	
	public static IntTag Read(NbtStreamReader reader) {
		return new IntTag(reader.ReadInt());
	}

	public override string GetTypeName() {
		return "Int";
	}

	public override string StringifyValue(int indentation) {
		return Convert.ToString(_value);
	}
}
