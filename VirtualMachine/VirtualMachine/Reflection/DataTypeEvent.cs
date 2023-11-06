using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Reflection
{
	public class DataTypeEvent : DataTypeMember
	{
		#region Properties

		//protected internal const MemoryOffset FieldOffsetXxx = FieldsCountOfMemberClass + 0;

		protected internal const MemoryOffset FieldsCountOfDataTypeEventClass = 0;
		protected internal const MemoryOffset TotalFieldsCountOfDataTypeEventClass = TotalFieldsCountOfDataTypeMemberClass + FieldsCountOfDataTypeEventClass;

		public override int GetReferencedDataSize()
		{
			return TotalFieldsCountOfDataTypeEventClass;
		}

		#endregion

		#region Constructors

		public DataTypeEvent(Core.Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{ }

		#endregion
	}
}
