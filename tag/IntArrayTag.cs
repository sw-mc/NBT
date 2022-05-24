using StreamReader = SkyWing.Binary.StreamReader;
using StreamWriter = SkyWing.Binary.StreamWriter;

namespace SkyWing.NBT.Tag; 

public class IntArrayTag : ImmutableTag {

	private readonly int[] _value;
	
	public IntArrayTag(int[] value) {
		_value = value;
	}
	
	public override int[] GetValue() {
		return _value;
	}

	public override byte GetTagType() {
		return (byte)TagType.IntArray;
	}

	public override void Write(StreamWriter writer) {
		writer.WriteIntArray(_value);
	}
	
	public static IntArrayTag Read(StreamReader reader) {
		return new IntArrayTag(reader.ReadIntArray());
	}

	public override string GetTypeName() {
		return "IntArray";
	}

	public override string StringifyValue(int indentation) {
		return "["+String.Join(',', _value)+"]";
	}
}