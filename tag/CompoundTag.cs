using System.Text;
using SkyWing.NBT.Serialization;
using SkyWing.NBT.Utils;

namespace SkyWing.NBT.Tag;

public class CompoundTag : Tag {

	private readonly Dictionary<string, Tag> _value = new();

	private static CompoundTag Create() {
		return new CompoundTag();
	}

	public int Count => _value.Count;

	public override Dictionary<string, Tag> GetValue() {
		return _value;
	}

	public Tag? GetTag(string name) {
		return _value.TryGetValue(name, out var tag) ? tag : null;
	}

	public ListTag GetListTag(string name) {
		var tag = GetTag(name);
		if (tag == null || tag.GetType() != typeof(ListTag))
			throw new UnexpectedTagTypeException("Expected a tag of type ListTag, got " +
			                                     (tag != null ? tag.GetTagType() : "NULL"));

		return (ListTag) tag;
	}

	public CompoundTag GetCompoundTag(string name) {
		var tag = GetTag(name);
		if (tag == null || tag.GetType() != typeof(CompoundTag))
			throw new UnexpectedTagTypeException("Expected a tag of type CompoundTag, got " +
			                                     (tag != null ? tag.GetTagType() : "NULL"));

		return (CompoundTag) tag;
	}

	public CompoundTag SetTag(string name, Tag tag) {
		_value[name] = tag;
		return this;
	}

	public void RemoveTag(string name) {
		_value.Remove(name);
	}

	private object GetTagValue(string name, Type expectedType, object? def = null) {
		var tag = GetTag(name);
		if (tag?.GetType() == expectedType)
			return tag.GetValue();
		
		if (tag != null)
			throw new UnexpectedTagTypeException("Expected a tag of type CompoundTag, got " + tag.GetTagType());
		
		if (def == null)
			throw new NoSuchTagException("Tag " + name + " does not exist");
		
		return def;
	}
	
	public byte GetByte(string name, byte? def = null) {
		return (byte) GetTagValue(name, typeof(ByteTag), def);
	}
	
	public short GetShort(string name, short? def = null) {
		return (short) GetTagValue(name, typeof(ShortTag), def);
	}
	
	public int GetInt(string name, int? def = null) {
		return (int) GetTagValue(name, typeof(IntTag), def);
	}
	
	public long GetLong(string name, long? def = null) {
		return (long) GetTagValue(name, typeof(LongTag), def);
	}
	
	public float GetFloat(string name, float? def = null) {
		return (float) GetTagValue(name, typeof(FloatTag), def);
	}
	
	public double GetDouble(string name, double? def = null) {
		return (double) GetTagValue(name, typeof(DoubleTag), def);
	}

	public byte[] GetByteArray(string name, byte[]? def = null) {
		return (byte[]) GetTagValue(name, typeof(ByteArrayTag), def);
	}
	
	public string GetString(string name, string? def = null) {
		return (string) GetTagValue(name, typeof(StringTag), def);
	}
	
	public int[] GetIntArray(string name, int[]? def = null) {
		return (int[]) GetTagValue(name, typeof(IntArrayTag), def);
	}
	
	public long[] GetLongArray(string name, long[]? def = null) {
		return (long[]) GetTagValue(name, typeof(LongArrayTag), def);
	}
	
	public void SetByte(string name, sbyte value) {
		SetTag(name, new ByteTag(value));
	}
	
	public void SetShort(string name, short value) {
		SetTag(name, new ShortTag(value));
	}
	
	public void SetInt(string name, int value) {
		SetTag(name, new IntTag(value));
	}
	
	public void SetLong(string name, long value) {
		SetTag(name, new LongTag(value));
	}
	
	public void SetFloat(string name, float value) {
		SetTag(name, new FloatTag(value));
	}
	
	public void SetDouble(string name, double value) {
		SetTag(name, new DoubleTag(value));
	}

	public void SetByteArray(string name, byte[] value) {
		SetTag(name, new ByteArrayTag(value));
	}
	
	public void SetString(string name, string value) {
		SetTag(name, new StringTag(value));
	}
	
	public void SetIntArray(string name, int[] value) {
		SetTag(name, new IntArrayTag(value));
	}
	
	public void SetLongArray(string name, long[] value) {
		SetTag(name, new LongArrayTag(value));
	}

	public override byte GetTagType() {
		return (byte)TagType.Compound;
	}

	public override void Write(NbtStreamWriter writer) {
		foreach (var (key, value) in _value) {
			writer.WriteUnsignedByte(value.GetTagType());
			writer.WriteString(key);
			value.Write(writer);
		}

		writer.WriteUnsignedByte((byte)TagType.Compound);
	}

	public static CompoundTag Read(NbtStreamReader reader, ReaderTracker tracker) {
		var result = new CompoundTag();
		tracker.ProtectDepth(_ => {
			var tagType = reader.ReadUnsignedByte();
			while (tagType != (byte)TagType.End) {
				var name = reader.ReadString();
				var tag = NBT.CreateTag((TagType)tagType, reader, tracker);
				result.SetTag(name, tag);
				tagType = reader.ReadUnsignedByte();
			}
		});

		return result;
	}

	public override string GetTypeName() {
		return "Compound";
	}

	public override string StringifyValue(int indentation) {
		var sb = new StringBuilder();
		foreach (var (key, value) in _value) {
			sb.AppendLine($"{new string(' ', indentation)}\"{key}\": {value.StringifyValue(indentation + 2)}");
		}

		return sb.AppendLine(new string(' ', indentation)).ToString();
	}

	protected override CompoundTag MakeCopy() {
		return (CompoundTag) MemberwiseClone();
	}

	public new bool Equals(Tag that) {
		if (that is not CompoundTag tag || Count != tag.Count)
			return false;

		foreach (var (key, value) in _value) {
			var other = ((CompoundTag) that).GetTag(key);
			if (other != null || !value.Equals(other))
				return false;
		}

		return true;
	}

	public CompoundTag Merge(CompoundTag other) {
		var result = MakeCopy();
		
		foreach (var (key, value) in other._value) {
			if (result._value.ContainsKey(key))
				continue;

			result._value.Add(key, value);
		}

		return result;
	}
}