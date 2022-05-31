using VirtualMachine.Reflection;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class DataType : ClassInstance
	{
		#region Properties

		protected internal const MemoryOffset FieldOffsetBaseType = FieldsCountOfObjectClass + 0;
		protected internal const MemoryOffset FieldOffsetFields = FieldsCountOfObjectClass + 1;
		protected internal const MemoryOffset FieldOffsetMethods = FieldsCountOfObjectClass + 2;
		protected internal const MemoryOffset FieldOffsetProperties = FieldsCountOfObjectClass + 3;
		protected internal const MemoryOffset FieldOffsetEvents = FieldsCountOfObjectClass + 4;
		protected internal const MemoryOffset FieldOffsetConstructors = FieldsCountOfObjectClass + 5;
		protected internal const MemoryOffset FieldOffsetName = FieldsCountOfObjectClass + 6;

		protected internal const MemoryOffset FieldsCountOfDataTypeClass = 7;
		protected internal const MemoryOffset TotalFieldsCountOfDataTypeClass = TotalFieldsCountOfObjectClass + FieldsCountOfDataTypeClass;

		public override int DataSize
		{ get { return TotalFieldsCountOfDataTypeClass; } }

		public DataType BaseType
		{ get { return GetFieldValue<DataType>(FieldOffsetBaseType); } }

		public Array<DataTypeField> Fields
		{ get { return GetFieldValue<Array<DataTypeField>>(FieldOffsetFields); } }

		public Array<DataTypeMethod> Methods
		{ get { return GetFieldValue<Array<DataTypeMethod>>(FieldOffsetMethods); } }

		public Array<DataTypeProperty> Properties
		{ get { return GetFieldValue<Array<DataTypeProperty>>(FieldOffsetProperties); } }

		public Array<DataTypeEvent> Events
		{ get { return GetFieldValue<Array<DataTypeEvent>>(FieldOffsetEvents); } }

		public Array<DataTypeConstructor> Constructors
		{ get { return GetFieldValue<Array<DataTypeConstructor>>(FieldOffsetConstructors); } }

		public String Name
		{ get { return GetFieldValue<String>(FieldOffsetName); } }

		#endregion

		#region Conctructors

		public DataType(Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{ }

		#endregion
	}
}
