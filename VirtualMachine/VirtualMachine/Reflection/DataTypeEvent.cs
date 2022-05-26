namespace VirtualMachine.Reflection
{
	public class DataTypeEvent : DataTypeMember
	{
		public DataTypeEvent()
			: base(DataTypeEventDataType)
		{ }

		#region Metadata

		public static readonly Core.DataType DataTypeEventDataType = new Core.DataType(DataTypeMemberDataType, new DataTypeField[0]);

		#endregion
	}
}
