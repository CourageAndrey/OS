namespace VirtualMachine.Reflection
{
	public class DataTypeConstructor : DataTypeMember
	{
		public DataTypeConstructor()
			: base(DataTypeConstructorDataType)
		{ }

		#region Metadata

		public static readonly Core.DataType DataTypeConstructorDataType = new Core.DataType(DataTypeMemberDataType, new DataTypeField[0]);

		#endregion
	}
}
