using System.Globalization;
using SkyWing.NBT.Serialization;

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

	public override void Write(NbtStreamWriter writer) {
		writer.WriteFloat(_value);
	}
	
	public static FloatTag Read(NbtStreamReader reader) {
		return new FloatTag(reader.ReadFloat());
	}

	public override string GetTypeName() {
		return "Float";
	}

	public override string StringifyValue(int indentation) {
		return Convert.ToString(_value, CultureInfo.InvariantCulture);
	}
}