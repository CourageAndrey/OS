namespace VirtualMachine.Core
{
	public class Constructor : ClassMember
	{
		internal Constructor(Class ofClass)
			: base(new String("_"), Class.ConstructorClass, ofClass)
		{ }
	}
}
