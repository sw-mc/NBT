using StreamReader = SkyWing.Binary.StreamReader;
using StreamWriter = SkyWing.Binary.StreamWriter;

namespace SkyWing.NBT.Tag;

public class IntTag : ImmutableTag {

	private readonly int _value;
	
	public IntTag(int value) {
		_value = value;
	}
	
	public override object GetValue() {
		return _value;
	}

	public override byte GetTagType() {
		return (byte)TagType.Int;
	}

	public override void Write(StreamWriter writer) {
		writer.WriteInt(_value);
	}
	
	public static IntTag Read(StreamReader reader) {
		return new IntTag(reader.ReadInt32());
	}

	public override string GetTypeName() {
		return "Int";
	}

	public override string StringifyValue(int indentation) {
		return Convert.ToString(_value);
	}
}
