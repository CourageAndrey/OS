using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Reflection
{
	public class DataTypeConstructor : DataTypeMember
	{
		#region Properties

		//protected internal const MemoryOffset FieldOffsetXxx = FieldsCountOfMemberClass + 0;

		protected internal const MemoryOffset FieldsCountOfDataTypeConstructorClass = 0;
		protected internal const MemoryOffset TotalFieldsCountOfDataTypeConstructorClass = TotalFieldsCountOfDataTypeMemberClass + FieldsCountOfDataTypeConstructorClass;

		#endregion

		#region Conctructors

		public DataTypeConstructor(Core.Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{ }

		#endregion
	}
}
