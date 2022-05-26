namespace VirtualMachine.Reflection
{
	public abstract class DataTypeMember : Core.Object
	{
		protected DataTypeMember(Core.DataType dataType)
			: base(dataType)
		{ }

		#region Metadata

		public static readonly Core.DataType DataTypeMemberDataType = new Core.DataType(ObjectDataType, new DataTypeField[0]);

		#endregion
	}
}
