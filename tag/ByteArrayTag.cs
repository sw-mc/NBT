using StreamReader = SkyWing.Binary.StreamReader;
using StreamWriter = SkyWing.Binary.StreamWriter;

namespace SkyWing.NBT.Tag; 

public class ByteArrayTag : ImmutableTag{
	
	private readonly byte[] _value;
	
	public ByteArrayTag(byte[] value){
		_value = value;
	}
	
	public override byte[] GetValue() {
		return _value;
	}

	public override byte GetTagType() {
		return (byte)TagType.ByteArray;
	}

	public override void Write(StreamWriter writer) {
		writer.WriteByteArray(_value);
	}
	
	public static ByteArrayTag Read(StreamReader reader){
		return new ByteArrayTag(reader.ReadByteArray());
	}

	public override string GetTypeName() {
		return "ByteArray";
	}

	public override string StringifyValue(int indentation) {
		return "b64:" + Convert.ToBase64String(_value);
	}
}