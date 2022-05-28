using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Reflection
{
	public class DataTypeProperty : DataTypeMember
	{
		#region Properties

		//protected internal const MemoryOffset FieldOffsetXxx = FieldsCountOfMemberClass + 0;

		protected internal const MemoryOffset FieldsCountOfDataTypePropertyClass = 0;
		protected internal const MemoryOffset TotalFieldsCountOfDataTypePropertyClass = TotalFieldsCountOfDataTypeMemberClass + FieldsCountOfDataTypePropertyClass;

		#endregion

		#region Conctructors

		public DataTypeProperty(Core.Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{ }

		#endregion
	}
}
