namespace VirtualMachine.Core
{
	public class Property : ClassMember
	{
		public Class DataType
		{ get { return _dataType; } }

		public Boolean CanGet
		{ get { return Boolean.FromSystem(_getter != null); } }

		public Boolean CanSet
		{ get { return Boolean.FromSystem(_setter != null); } }

		private readonly Class _dataType;
		private readonly System.Func<Object, Object> _getter;
		private readonly System.Action<Object, Object> _setter;

		internal Property(
			String name,
			Class ofClass,
			Class dataType,
			System.Func<Object, Object> getter,
			System.Action<Object, Object> setter)
			: base(name, Class.PropertyClass, ofClass)
		{
			_dataType = dataType;
			_getter = getter;
			_setter = setter;
		}

		public Object GetValue(Object instance)
		{
#warning Check for null and wrong input object type
			return _getter(instance);
		}

		public void SetValue(Object instance, Object value)
		{
#warning Check for null and wrong input object type
			_setter(instance, value);
		}
	}
}
