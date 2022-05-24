using System.Globalization;
using StreamReader = SkyWing.Binary.StreamReader;
using StreamWriter = SkyWing.Binary.StreamWriter;

namespace SkyWing.NBT.Tag; 

public class FloatTag : ImmutableTag {
	
	private readonly float _value;
	
	public FloatTag(float value) {
		_value = value;
	}
	
	public override object GetValue() {
		return _value;
	}

	public override byte GetTagType() {
		return (byte)TagType.Float;
	}

	public override void Write(StreamWriter writer) {
		writer.WriteFloat(_value);
	}
	
	public static FloatTag Read(StreamReader reader) {
		return new FloatTag(reader.ReadSingle());
	}

	public override string GetTypeName() {
		return "Float";
	}

	public override string StringifyValue(int indentation) {
		return Convert.ToString(_value, CultureInfo.InvariantCulture);
	}
}