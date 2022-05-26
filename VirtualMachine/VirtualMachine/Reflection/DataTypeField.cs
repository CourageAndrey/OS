namespace VirtualMachine.Reflection
{
	public class DataTypeField : DataTypeMember
	{
		public DataTypeField()
			: base(DataTypeFieldDataType)
		{ }

		#region Metadata

		public static readonly Core.DataType DataTypeFieldDataType = new Core.DataType(DataTypeMemberDataType, new DataTypeField[0]);

		#endregion
	}
}
