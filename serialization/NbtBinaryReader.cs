using SkyWing.Binary;
using SkyWing.NBT.Tag;
using SkyWing.NBT.Utils;

namespace SkyWing.NBT.Serialization;

public class NbtBinaryReader : Reader {

    public NbtBinaryReader(Stream input, bool bigEndian) : base(input, bigEndian) {
    }
    
    public TagType ReadTagType() {
        int type = ReadByte();
        if (type > (int) TagType.LongArray) {
            throw new NbtDataException("NBT tag type out of range: " + type);
        }

        return (TagType) type;
    }
}