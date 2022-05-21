using SkyWing.NBT.Serializer;
using SkyWing.NBT.Tag;
using SkyWing.NBT.Utils;

namespace SkyWing.NBT; 

public static class NBT {
	
	public const int TAG_End = 0;
	public const int TAG_Byte = 1;
	public const int TAG_Short = 2;
	public const int TAG_Int = 3;
	public const int TAG_Long = 4;
	public const int TAG_Float = 5;
	public const int TAG_Double = 6;
	public const int TAG_ByteArray = 7;
	public const int TAG_String = 8;
	public const int TAG_List = 9;
	public const int TAG_Compound = 10;
	public const int TAG_IntArray = 11;

	public static Tag.Tag CreateTag(int type, NbtStreamReader reader) {
		return type switch {
			TAG_Byte => ByteTag.Read(reader),
			TAG_Short => ShortTag.Read(reader),
			TAG_Int => IntTag.Read(reader),
			TAG_Long => LongTag.Read(reader),
			TAG_Float => FloatTag.Read(reader),
			TAG_Double => DoubleTag.Read(reader),
			TAG_ByteArray => ByteArrayTag.Read(reader),
			TAG_String => StringTag.Read(reader),
			TAG_List => ListTag.Read(reader),
			TAG_Compound => CompoundTag.Read(reader),
			TAG_IntArray => IntArrayTag.Read(reader),
			_ => throw new NoSuchTagException("Unknown NBT tag type: " + type)
		};
	}
}