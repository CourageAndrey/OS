using VirtualMachine.Reflection;

namespace VirtualMachine.Core
{
	public class DataType : Object
	{
		#region Properties

		public DataType BaseType
		{ get; internal set; }

		public System.Collections.Generic.ICollection<DataTypeField> Fields
		{ get; }

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

		internal DataType(DataType baseType, System.Collections.Generic.ICollection<DataTypeField> fields)
			: base(DataTypeDataType)
		{
			BaseType = baseType;
			Fields = fields;
		}

		#endregion

		#region Metadata

		public static readonly DataType DataTypeDataType = new DataType(ObjectDataType, new DataTypeField[0]);

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
