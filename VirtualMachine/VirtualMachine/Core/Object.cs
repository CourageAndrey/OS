using VirtualMachine.Reflection;

namespace VirtualMachine.Core
{
	public class Object
	{
		#region Properties

		public DataType DataType
		{ get { return (DataType) _data[0]; } }

		protected System.Collections.Generic.IList<DataTypeField> _fields;
		protected System.Collections.Generic.IList<Object> _data;

		#endregion

		#region Conctructors

		protected Object(DataType dataType)
		{
			_fields = dataType != null ? dataType.GetAllFields() as System.Collections.Generic.IList<DataTypeField> : new DataTypeField[0];
			if (_fields.Count > 0)
			{ // normal object creation
				_data = new Object[_fields.Count];
				_data[0] = dataType;
			}
			else
			{ // create first objects
				_data = new Object[0];
			}
		}

		public Object()
			: this(ObjectDataType)
		{ }

		#endregion

		#region Metadata

		internal static readonly DataTypeField FieldDataType;

		public static readonly DataType ObjectDataType;

		static Object()
		{
			ObjectDataType = new DataType(null, new System.Collections.Generic.List<DataTypeField>());

			FieldDataType = new DataTypeField();
			ObjectDataType.Fields.Add(FieldDataType);

			AddFieldWithValue(ObjectDataType, FieldDataType, ObjectDataType);
			AddFieldWithValue(DataType.DataTypeDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeMember.DataTypeMemberDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeField.DataTypeFieldDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeMethod.DataTypeMethodDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeProperty.DataTypePropertyDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeConstructor.DataTypeConstructorDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeEvent.DataTypeEventDataType, FieldDataType, DataType.DataTypeDataType);
		}

		protected static void AddFieldWithValue(Object self, DataTypeField field, Object value)
		{
			self._fields = new System.Collections.Generic.List<DataTypeField>(self._fields) { field }.ToArray();
			self._data = new System.Collections.Generic.List<Object>(self._data) { value }.ToArray();
		}

		#endregion
	}
}
