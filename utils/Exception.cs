namespace SkyWing.NBT.Utils; 

public class NbtException : Exception
{
	public NbtException(string message) : base(message)
	{
	}
}

public class NbtDataException : NbtException
{
	public NbtDataException(string message) : base(message)
	{
	}
}

public class UnexpectedTagTypeException : NbtDataException
{
	public UnexpectedTagTypeException(string message) : base(message)
	{
	}
}

public class NoSuchTagException : NbtDataException
{
	public NoSuchTagException(string message) : base(message)
	{
	}
}