using SkyWing.NBT.Tag;

namespace DefaultNamespace; 

public class TreeRoot {

	public Tag Root { get; }
	public string Name { get; }
	
	public TreeRoot(Tag root, string name) {
		Root = root;
		Name = name;
	}

	public bool Equals(TreeRoot that) {
		return Name.Equals(that.Name) && Root.Equals(that.Root);
	}

	public override string ToString() {
		return "ROOT {\n" + (Name != "" ? "\"" + Name + "\" => " : "") + Root._ToString(1) + "\n}";
	}
}