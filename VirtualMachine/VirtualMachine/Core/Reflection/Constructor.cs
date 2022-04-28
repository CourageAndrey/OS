using VirtualMachine.Core.DataTypes;

namespace VirtualMachine.Core.Reflection
{
	public class Constructor : ClassMember
	{
		internal Constructor(Class ofClass)
			: base(new String("_"), Class.ConstructorClass, ofClass)
		{ }
	}
}
