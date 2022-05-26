using VirtualMachine.Reflection;

namespace VirtualMachine.Core
{
	public class Object
	{
		#region Properties

		public DataType DataType
		{
			get { return (DataType) _data[0]; }
			private set { _data[0] = value; }
		}

		protected System.Collections.Generic.IList<DataTypeField> _fields;
		protected System.Collections.Generic.IList<Object> _data;

		#endregion

		#region Conctructors

		public Object()
		{
			_fields = new DataTypeField[0];
			_data = new Object[0];
		}

		public static ObjectT Create<ObjectT>(DataType type)
			where ObjectT : Object, new()
		{
			var result = new ObjectT();
			result.CallConstructor(type);
			return result;
		}

		protected void CallConstructor(DataType type)
		{
			_fields = type.GetAllFields();
			_data = new Object[_fields.Count];
			DataType = type;
		}

		#endregion

		#region Metadata

		internal static readonly DataTypeField FieldDataType;

		public static readonly DataType ObjectDataType;

		static Object()
		{
			ObjectDataType = new DataType();

			FieldDataType = new DataTypeField();
			ObjectDataType.Fields.Add(FieldDataType);

			DataType.FieldBaseType = new DataTypeField();
			DataType.DataTypeDataType.Fields.Add(FieldDataType);

			AddFieldWithValue(ObjectDataType, FieldDataType, ObjectDataType);
			AddFieldWithValue(DataType.DataTypeDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeMember.DataTypeMemberDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeField.DataTypeFieldDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeMethod.DataTypeMethodDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeProperty.DataTypePropertyDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeConstructor.DataTypeConstructorDataType, FieldDataType, DataType.DataTypeDataType);
			AddFieldWithValue(DataTypeEvent.DataTypeEventDataType, FieldDataType, DataType.DataTypeDataType);

			AddFieldWithValue(ObjectDataType, DataType.FieldBaseType, null);
			AddFieldWithValue(DataType.DataTypeDataType, DataType.FieldBaseType, ObjectDataType);
			AddFieldWithValue(DataTypeMember.DataTypeMemberDataType, DataType.FieldBaseType, ObjectDataType);
			AddFieldWithValue(DataTypeField.DataTypeFieldDataType, DataType.FieldBaseType, DataTypeMember.DataTypeMemberDataType);
			AddFieldWithValue(DataTypeMethod.DataTypeMethodDataType, DataType.FieldBaseType, DataTypeMember.DataTypeMemberDataType);
			AddFieldWithValue(DataTypeProperty.DataTypePropertyDataType, DataType.FieldBaseType, DataTypeMember.DataTypeMemberDataType);
			AddFieldWithValue(DataTypeConstructor.DataTypeConstructorDataType, DataType.FieldBaseType, DataTypeMember.DataTypeMemberDataType);
			AddFieldWithValue(DataTypeEvent.DataTypeEventDataType, DataType.FieldBaseType, DataTypeMember.DataTypeMemberDataType);
		}

		private static void AddFieldWithValue(Object self, DataTypeField field, Object value)
		{
			self._fields = new System.Collections.Generic.List<DataTypeField>(self._fields) { field }.ToArray();
			self._data = new System.Collections.Generic.List<Object>(self._data) { value }.ToArray();
		}

		#endregion
	}
}
