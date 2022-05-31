using SkyWing.NBT.Serialization;
using SkyWing.NBT.Utils;

namespace SkyWing.NBT.Tag; 

public abstract class Tag {

	/**
	 * Used for recursive cloning protection. (Cloning child tags)
	 */
	protected bool Cloning = false;

	public abstract object GetValue();

	public abstract byte GetTagType();
	
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
		if (Cloning)
			throw new NbtException("Recursive NBT tag dependency detected.");
		Cloning = true;

		var retval = MakeCopy();
		Cloning = false;
		retval.Cloning = false;
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