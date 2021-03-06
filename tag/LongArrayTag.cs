using StreamReader = SkyWing.Binary.StreamReader;
using StreamWriter = SkyWing.Binary.StreamWriter;

namespace SkyWing.NBT.Tag;

public class LongArrayTag : ImmutableTag {

	private readonly long[] _value;

	public LongArrayTag(long[] value) {
		_value = value;
	}

	public override long[] GetValue() {
		return _value;
	}

	public override byte GetTagType() {
		return (byte) TagType.LongArray;
	}

	public override void Write(StreamWriter writer) {
		writer.WriteLongArray(_value);
	}
	
	public static LongArrayTag Read(StreamReader reader) {
		return new LongArrayTag(reader.ReadLongArray());
	}

	public override string GetTypeName() {
		return "LongArray";
	}

	public override string StringifyValue(int indentation) {
		return "implode: " + string.Join('.', _value);
	}
}