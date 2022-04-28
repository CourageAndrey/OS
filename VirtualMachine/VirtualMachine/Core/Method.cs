namespace VirtualMachine.Core
{
	public class Method : ClassMember
	{
		internal Method(String name, Class ofClass)
			: base(name, Class.MethodClass, ofClass)
		{ }
	}
}
