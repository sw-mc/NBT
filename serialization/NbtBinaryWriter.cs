using System.Text;
using SkyWing.Binary;
using SkyWing.NBT.Tag;

namespace SkyWing.NBT.Serialization;

public class NbtBinaryWriter : Writer {
    
    public NbtBinaryWriter(Stream input, bool bigEndian) : base(input, bigEndian) {
    }

    public void Write(TagType value) {
        WriteByte((byte) value);
    }
}