using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Reflection
{
	public class DataTypeMethod : DataTypeMember
	{
		#region Properties

		//protected internal const MemoryOffset FieldOffsetXxx = FieldsCountOfMemberClass + 0;

		protected internal const MemoryOffset FieldsCountOfDataTypeMethodClass = 0;
		protected internal const MemoryOffset TotalFieldsCountOfDataTypeMethodClass = TotalFieldsCountOfDataTypeMemberClass + FieldsCountOfDataTypeMethodClass;

		public override int GetDataSize()
		{
			return TotalFieldsCountOfDataTypeMethodClass;
		}

		#endregion

		#region Constructors

		public DataTypeMethod(Core.Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{ }

		#endregion
	}
}
