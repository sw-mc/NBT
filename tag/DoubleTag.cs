using System.Globalization;
using StreamReader = SkyWing.Binary.StreamReader;
using StreamWriter = SkyWing.Binary.StreamWriter;

namespace SkyWing.NBT.Tag; 

public class DoubleTag : ImmutableTag {
	
	private readonly double _value;

	public DoubleTag(double value) {
		_value = value;
	}
	
	public override object GetValue() {
		return _value;
	}

	public override byte GetTagType() {
		return (byte)TagType.Double;
	}

	public override void Write(StreamWriter writer) {
		writer.WriteDouble(_value);
	}
	
	public static DoubleTag Read(StreamReader reader) {
		return new DoubleTag(reader.ReadDouble());
	}

	public override string GetTypeName() {
		return "Double";
	}

	public override string StringifyValue(int indentation) {
		return Convert.ToString(_value, CultureInfo.InvariantCulture);
	}
}