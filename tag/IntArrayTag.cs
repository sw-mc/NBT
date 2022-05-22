using SkyWing.NBT.Serialization;

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

	public override void Write(NbtStreamWriter writer) {
		writer.WriteIntArray(_value);
	}
	
	public static IntArrayTag Read(NbtStreamReader reader) {
		return new IntArrayTag(reader.ReadIntArray());
	}

	public override string GetTypeName() {
		return "IntArray";
	}

	public override string StringifyValue(int indentation) {
		return "["+String.Join(',', _value)+"]";
	}
}