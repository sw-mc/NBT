namespace SkyWing.NBT.Serialization; 

public abstract class BaseNbtStream {
	
	public Stream Stream { get; protected set; }

	public NbtStreamReader? Reader { get; protected set; }
	public NbtStreamWriter? Writer { get; protected set; }
	
	public BaseNbtStream(Stream stream) {
		Stream = stream;
	}
}