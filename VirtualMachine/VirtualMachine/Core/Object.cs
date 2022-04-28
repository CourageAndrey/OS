using VirtualMachine.Core.DataTypes;
using VirtualMachine.Core.Reflection;

namespace VirtualMachine.Core
{
	public class Object
	{
		public Class Class
		{ get { return _class; } }

		private readonly Class _class;

		internal readonly System.Collections.Generic.Dictionary<Field, Object> FieldValues = new System.Collections.Generic.Dictionary<Field, Object>();

		internal Object(Class @class)
		{
			_class = @class;
			_class.InitializeFieldValues(FieldValues);
		}

		public Object()
			: this(Class.ObjectClass)
		{ }

		public new virtual String ToString()
		{
			return Class.FullName;
		}

		public static readonly Object Null = new Object();
	}
}
