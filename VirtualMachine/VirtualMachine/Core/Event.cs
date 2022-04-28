namespace VirtualMachine.Core
{
	public class Event : ClassMember
	{
		internal Event(String name, Class ofClass)
			: base(name, Class.EventClass, ofClass)
		{ }
	}
}
