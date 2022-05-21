using SkyWing.NBT.Serializer;

namespace SkyWing.NBT.Tag; 

public class LongTag : ImmutableTag{
	
	private readonly long _value;
	
	public LongTag(long value){
		_value = value;
	}
	
	public override object GetValue() {
		return _value;
	}

	public override int GetTagType() {
		return NBT.TAG_Long;
	}

	public override void Write(NbtStreamWriter writer) {
		writer.WriteLong(_value);
	}
	
	public static LongTag Read(NbtStreamReader reader) {
		return new LongTag(reader.ReadLong());
	}

	public override string GetTypeName() {
		return "Long";
	}

	public override string StringifyValue(int indentation) {
		return Convert.ToString(_value);
	}
}