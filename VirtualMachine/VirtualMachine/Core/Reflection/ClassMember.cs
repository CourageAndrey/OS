using VirtualMachine.Core.DataTypes;

namespace VirtualMachine.Core.Reflection
{
	public abstract class ClassMember : Object
	{
		public String Name
		{ get { return _name; } }

		public Class OfClass
		{ get { return _ofClass; } }

		private readonly String _name;
		private readonly Class _ofClass;

		protected ClassMember(String name, Class memberClass, Class ofClass)
			: base(memberClass)
		{
			_name = name;
			_ofClass = ofClass;
		}

		protected ClassMember(String name)
			: base(Class.ClassMemberClass)
		{
			_name = name;
		}

		public override String ToString()
		{
			return Name;
		}
	}
}
