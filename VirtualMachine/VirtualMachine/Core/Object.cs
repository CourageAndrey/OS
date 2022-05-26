namespace VirtualMachine.Core
{
	public class Object
	{
		#region Properties

		public DataType DataType
		{ get; }

		#endregion

		#region Conctructors

		protected Object(DataType dataType)
		{
			DataType = dataType;
		}

		public Object()
			: this(DataType.ObjectDataType)
		{ }

		#endregion
	}
}
