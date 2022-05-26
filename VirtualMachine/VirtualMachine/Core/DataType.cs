using VirtualMachine.Reflection;

namespace VirtualMachine.Core
{
	public class DataType : Object
	{
		#region Properties

		public DataType BaseType
		{ get; }

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

		public DataType(DataType baseType)
			: base(DataTypeDataType)
		{
			if (baseType != null)
			{
				BaseType = baseType;
			}
			else
			{
				throw new System.ArgumentNullException(nameof(baseType));
			}
		}

		private DataType()
			: base(DataTypeDataType)
		{ }

		#endregion

		#region List

		public static readonly DataType ObjectDataType = new DataType();
		public static readonly DataType DataTypeDataType = new DataType(ObjectDataType);

		public static readonly DataType DataTypeMemberDataType = new DataType(ObjectDataType);
		public static readonly DataType DataTypeFieldDataType = new DataType(DataTypeMemberDataType);
		public static readonly DataType DataTypeMethodDataType = new DataType(DataTypeMemberDataType);
		public static readonly DataType DataTypePropertyDataType = new DataType(DataTypeMemberDataType);
		public static readonly DataType DataTypeEventDataType = new DataType(DataTypeMemberDataType);
		public static readonly DataType DataTypeConstructorDataType = new DataType(DataTypeMemberDataType);

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

		#endregion
	}
}
