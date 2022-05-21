using SkyWing.NBT.Serializer;
using SkyWing.NBT.Utils;

namespace SkyWing.NBT.Tag; 

public abstract class Tag {

	/**
	 * Used for recursive cloning protection. (Cloning child tags)
	 */
	protected bool cloning = false;

	public abstract object GetValue();

	public abstract int GetTagType();
	
	public abstract void Write(NbtStreamWriter writer);

	public override string ToString() {
		return _ToString(0);
	}

	public string _ToString(int indentation) {
		return "TAG_" + GetTypeName() + "=" + StringifyValue(indentation);
	}

	public abstract string GetTypeName();
	
	public abstract string StringifyValue(int indentation);

	public Tag SafeClone() {
		if (cloning)
			throw new NbtException("Recursive NBT tag dependency detected.");
		cloning = true;

		var retval = MakeCopy();
		cloning = false;
		retval.cloning = false;
		return retval;
	}

	protected abstract Tag MakeCopy();

	public bool Equals(Tag that) {
		return GetTagType() == that.GetTagType() && GetValue() == that.GetValue();
	}
}

public abstract class ImmutableTag : Tag {

	// Immutable tags do not need to be copied.
	protected override Tag MakeCopy() {
		return this;
	}
}