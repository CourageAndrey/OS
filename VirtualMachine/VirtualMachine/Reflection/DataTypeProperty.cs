namespace VirtualMachine.Reflection
{
	public class DataTypeProperty : DataTypeMember
	{
		public DataTypeProperty()
			: base(DataTypePropertyDataType)
		{ }

		#region Metadata

		public static readonly Core.DataType DataTypePropertyDataType = new Core.DataType(DataTypeMemberDataType, new DataTypeField[0]);

		#endregion
	}
}
