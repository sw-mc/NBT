using SkyWing.NBT.Serializer;

namespace SkyWing.NBT.Tag; 

public class StringTag : ImmutableTag {

	private readonly string _value;
	
	public StringTag(string value) {
		_value = value;
	}
	public override object GetValue() {
		return _value;
	}

	public override int GetTagType() {
		return NBT.TAG_String;
	}

	public override void Write(NbtStreamWriter writer) {
		writer.WriteString(_value);
	}
	
	public static StringTag Read(NbtStreamReader reader) {
		return new StringTag(reader.ReadString());
	}

	public override string GetTypeName() {
		return "String";
	}

	public override string StringifyValue(int indentation) {
		return '"' + _value + '"';
	}
}