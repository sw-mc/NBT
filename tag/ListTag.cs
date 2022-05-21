using System.Text;
using SkyWing.NBT.Serializer;
using SkyWing.NBT.Utils;

namespace SkyWing.NBT.Tag; 

public class ListTag : Tag {

	private int TagType { get; set; }

	private readonly List<Tag> _value = new();

	private ListTag(Tag[] value, int tagType = NBT.TAG_End){
		TagType = tagType;
		_value.AddRange(value);
	}
	
	public override Tag[] GetValue() {
		return _value.ToArray();
	}

	public object[] GetAllValues() {
		var result = new object[_value.Count];
		for (var i = 0; i < _value.Count; i++) {
			result[i] = _value[i].GetValue();
		}
		return result;
	}

	private int Count => _value.Count;

	public void Add(Tag tag) {
		CheckTagType(tag);
		_value.Add(tag);
	}
	
	public Tag GetLast() {
		var tag = _value[^1];
		_value.Remove(tag);
		return tag;
	}
	
	public void AddFirst(Tag tag) {
		CheckTagType(tag);
		_value.Insert(0, tag);
	}
	
	public Tag GetFirst() {
		var tag = _value[0];
		_value.Remove(tag);
		return tag;
	}
	
	public void Insert(int offset, Tag tag) {
		CheckTagType(tag);
		_value.Insert(offset, tag);
	}

	public Tag Get(int offset) {
		if (offset < 0 || _value.Count <= offset) {
			throw new IndexOutOfRangeException();
		}
		
		return _value[offset];
	}
	
	public Tag First() {
		return _value[0];
	}
	
	public Tag Last() {
		return _value[^1];
	}
	
	public void Set(int offset, Tag tag) {
		CheckTagType(tag);
		_value[offset] = tag;
	}
	
	public bool Empty() {
		return _value.Count == 0;
	}
	
	

	public override int GetTagType() {
		return NBT.TAG_List;
	}
	
	public override string GetTypeName() {
		return "ListTag";
	}

	private void CheckTagType(Tag tag) {
		var type = tag.GetTagType();
		if (type == TagType) return;
		if (TagType == NBT.TAG_End) {
			TagType = type;
		}
		else {
			throw new UnexpectedTagTypeException(
				"Invalid tag of type " + tag.GetTypeName() + " assigned to ListTag");
		}
	}

	public override void Write(NbtStreamWriter writer) {
		writer.WriteByte(Convert.ToByte(TagType));
		writer.WriteInt(Count);
		foreach (var tag in _value) {
			tag.Write(writer);
		}
	}

	public static ListTag Read(NbtStreamReader reader) {
		var tagType = reader.ReadByte();
		var count = reader.ReadInt();
		var tags = new Tag[count];

		if (count > 0) {
			if (tagType == NBT.TAG_End)
				throw new NbtDataException("Unexpected non-empty list of TAG_End");

			for (var i = 0; i < count; i++) {
				tags[i] = NBT.CreateTag(tagType, reader);

			}
		}
		else {
			tagType = NBT.TAG_End;
		}

		return new ListTag(tags, tagType);
	}


	public override string StringifyValue(int indentation) {
		var sb = new StringBuilder();
		_value.ForEach(value => {
			sb.AppendLine($"{new string(' ', indentation)}{value.StringifyValue(indentation + 2)}");
		});

		return sb.AppendLine(new string(' ', indentation)).ToString();
	}

	protected override ListTag MakeCopy() {
		return (ListTag) MemberwiseClone();
	}
}