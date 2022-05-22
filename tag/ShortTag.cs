﻿using SkyWing.NBT.Serialization;

namespace SkyWing.NBT.Tag; 

public class ShortTag : ImmutableTag {
	
	private readonly short _value;
	
	public ShortTag(short value) {
		_value = value;
	}
	
	public override object GetValue() {
		return _value;
	}

	public override byte GetTagType() {
		return (byte) TagType.Short;
	}

	public override void Write(NbtStreamWriter writer) {
		writer.WriteSignedShort(_value);
	}
	
	public static ShortTag Read(NbtStreamReader reader) {
		return new ShortTag(reader.ReadInt16());
	}

	public override string GetTypeName() {
		return "Short";
	}

	public override string StringifyValue(int indentation) {
		return Convert.ToString(_value);
	}
}