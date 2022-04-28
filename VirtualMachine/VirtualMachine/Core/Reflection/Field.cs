using VirtualMachine.Core.DataTypes;

namespace VirtualMachine.Core.Reflection
{
	public class Field : ClassMember
	{
		public Class DataType
		{ get { return _dataType; } }

		private readonly Class _dataType;

		internal Field(String name, Class dataType, Class ofClass)
			: base(name, Class.FieldClass, ofClass)
		{
			_dataType = dataType;
		}

		public Object GetValue(Object instance)
		{
#warning Check for null and wrong input object type
			return instance.FieldValues[this];
		}

		public void SetValue(Object instance, Object value)
		{
#warning Check for null and wrong input object type
			instance.FieldValues[this] = value;
		}
	}
}
