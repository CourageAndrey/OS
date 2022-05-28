using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Reflection
{
	public abstract class DataTypeMember : Core.ClassInstance
	{
		#region Properties

		//protected internal const MemoryOffset FieldOffsetXxx = FieldsCountOfObjectClass + 0;

		protected internal const MemoryOffset FieldsCountOfDataTypeMemberClass = 0;
		protected internal const MemoryOffset TotalFieldsCountOfDataTypeMemberClass = TotalFieldsCountOfObjectClass + FieldsCountOfDataTypeMemberClass;

		#endregion

		#region Conctructors

		public DataTypeMember(Core.Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{ }

		#endregion
	}
}
