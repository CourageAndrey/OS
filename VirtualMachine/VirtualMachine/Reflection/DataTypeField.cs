using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Reflection
{
	public class DataTypeField : DataTypeMember
	{
		#region Properties

		//protected internal const MemoryOffset FieldOffsetXxx = FieldsCountOfMemberClass + 0;

		protected internal const MemoryOffset FieldsCountOfDataTypeFieldClass = 0;
		protected internal const MemoryOffset TotalFieldsCountOfDataTypeFieldClass = TotalFieldsCountOfDataTypeMemberClass + FieldsCountOfDataTypeFieldClass;

		public override int GetDataSize()
		{
			return TotalFieldsCountOfDataTypeFieldClass;
		}

		#endregion

		#region Conctructors

		public DataTypeField(Core.Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{ }

		#endregion
	}
}
