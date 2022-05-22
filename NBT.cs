using SkyWing.NBT.Serialization;
using SkyWing.NBT.Tag;
using SkyWing.NBT.Utils;

namespace SkyWing.NBT; 

public static class NBT {

	public static Tag.Tag CreateTag(TagType type, NbtStreamReader reader) {
		return type switch {
			TagType.Byte => ByteTag.Read(reader),
			TagType.Short => ShortTag.Read(reader),
			TagType.Int => IntTag.Read(reader),
			TagType.Long => LongTag.Read(reader),
			TagType.Float => FloatTag.Read(reader),
			TagType.Double => DoubleTag.Read(reader),
			TagType.ByteArray => ByteArrayTag.Read(reader),
			TagType.String => StringTag.Read(reader),
			TagType.List => ListTag.Read(reader),
			TagType.Compound => CompoundTag.Read(reader),
			TagType.IntArray => IntArrayTag.Read(reader),
			TagType.LongArray => LongArrayTag.Read(reader),
			_ => throw new NoSuchTagException("Unknown NBT tag type: " + type)
		};
	}
}