namespace VirtualMachine.Reflection
{
	public class DataTypeMethod : DataTypeMember
	{
		public DataTypeMethod()
			: base(DataTypeMethodDataType)
		{ }

		#region Metadata

		public static readonly Core.DataType DataTypeMethodDataType = new Core.DataType(DataTypeMemberDataType, new DataTypeField[0]);

		#endregion
	}
}
