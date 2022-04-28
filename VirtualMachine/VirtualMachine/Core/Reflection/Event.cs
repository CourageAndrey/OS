using VirtualMachine.Core.DataTypes;

namespace VirtualMachine.Core.Reflection
{
	public class Event : ClassMember
	{
		internal Event(String name, Class ofClass)
			: base(name, Class.EventClass, ofClass)
		{ }
	}
}
