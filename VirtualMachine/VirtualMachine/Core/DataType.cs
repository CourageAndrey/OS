namespace VirtualMachine.Core
{
	public class DataType : Object
	{
		#region Properties

		public DataType BaseType
		{ get; }

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

		#endregion
	}
}
