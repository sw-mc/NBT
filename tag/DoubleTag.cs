using System.Globalization;
using SkyWing.NBT.Serialization;

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

	public override void Write(NbtStreamWriter writer) {
		writer.WriteDouble(_value);
	}
	
	public static DoubleTag Read(NbtStreamReader reader) {
		return new DoubleTag(reader.ReadDouble());
	}

	public override string GetTypeName() {
		return "Double";
	}

	public override string StringifyValue(int indentation) {
		return Convert.ToString(_value, CultureInfo.InvariantCulture);
	}
}