using VirtualMachine.Core.DataTypes;

namespace VirtualMachine.Core.Reflection
{
	public class Method : ClassMember
	{
		internal Method(String name, Class ofClass)
			: base(name, Class.MethodClass, ofClass)
		{ }
	}
}
