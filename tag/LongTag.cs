using StreamReader = SkyWing.Binary.StreamReader;
using StreamWriter = SkyWing.Binary.StreamWriter;

namespace SkyWing.NBT.Tag; 

public class LongTag : ImmutableTag{
	
	private readonly long _value;
	
	public LongTag(long value){
		_value = value;
	}
	
	public override object GetValue() {
		return _value;
	}

	public override byte GetTagType() {
		return (byte) TagType.Long;
	}

	public override void Write(StreamWriter writer) {
		writer.WriteLong(_value);
	}
	
	public static LongTag Read(StreamReader reader) {
		return new LongTag(reader.ReadInt64());
	}

	public override string GetTypeName() {
		return "Long";
	}

	public override string StringifyValue(int indentation) {
		return Convert.ToString(_value);
	}
}