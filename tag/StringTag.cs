
using SkyWing.Binary;
using StreamReader = SkyWing.Binary.StreamReader;
using StreamWriter = SkyWing.Binary.StreamWriter;

namespace SkyWing.NBT.Tag; 

public class StringTag : ImmutableTag {

	private readonly string _value;
	
	public StringTag(string value) {
		_value = value;
	}
	public override object GetValue() {
		return _value;
	}

	public override byte GetTagType() {
		return (byte) TagType.Short;
	}

	public override void Write(StreamWriter writer) {
		writer.WriteString(_value);
	}
	
	public static StringTag Read(StreamReader reader) {
		return new StringTag(reader.ReadString());
	}

	public override string GetTypeName() {
		return "String";
	}

	public override string StringifyValue(int indentation) {
		return '"' + _value + '"';
	}
}