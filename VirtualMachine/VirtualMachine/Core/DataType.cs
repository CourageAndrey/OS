using VirtualMachine.Reflection;

namespace VirtualMachine.Core
{
	public class DataType : Object
	{
		#region Properties

		public DataType BaseType
		{
			get { return (DataType) _data[ObjectDataType.Fields.Count + 0]; }
			internal set { _data[ObjectDataType.Fields.Count + 0] = value; }
		}

		public System.Collections.Generic.ICollection<DataTypeField> Fields
		{ get; } = new System.Collections.Generic.List<DataTypeField>();

		public System.Collections.Generic.ICollection<DataTypeMethod> Methods
		{ get; } = new System.Collections.Generic.List<DataTypeMethod>();

		public System.Collections.Generic.ICollection<DataTypeProperty> Properties
		{ get; } = new System.Collections.Generic.List<DataTypeProperty>();

		public System.Collections.Generic.ICollection<DataTypeEvent> Events
		{ get; } = new System.Collections.Generic.List<DataTypeEvent>();

		public System.Collections.Generic.ICollection<DataTypeConstructor> Constructors
		{ get; } = new System.Collections.Generic.List<DataTypeConstructor>();

		#endregion

		#region Conctructors

		public static DataType Create(DataType baseType)
		{
			var result = Create<DataType>(DataTypeDataType);
			result.BaseType = baseType;
			return result;
		}

		#endregion

		#region Metadata

		public static readonly DataType DataTypeDataType = new DataType();

		internal static DataTypeField FieldBaseType;

		#endregion

		#region Public API

		public System.Collections.Generic.IEnumerable<DataTypeMember> GetAllMembers()
		{
			foreach (var field in Fields)
			{
				yield return field;
			}

			foreach (var method in Methods)
			{
				yield return method;
			}

			foreach (var property in Properties)
			{
				yield return property;
			}

			foreach (var @event in Events)
			{
				yield return @event;
			}

			foreach (var constructor in Constructors)
			{
				yield return constructor;
			}
		}

		internal System.Collections.Generic.List<DataTypeField> GetAllFields()
		{
			var fields = BaseType != null
				? BaseType.GetAllFields()
				: new System.Collections.Generic.List<DataTypeField>();

			fields.AddRange(Fields);

			return fields;
		}

		#endregion
	}
}
