using VirtualMachine.Core;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Tests
{
	internal class UnknownStructure : ValueObject
	{
		public override DataType GetDataType()
		{
			return Memory.ObjectDataType;
		}

		internal override System.Collections.Generic.List<MemoryWord> Serialize(Memory memory, int address)
		{
			return new System.Collections.Generic.List<MemoryWord> { 0 };
		}
	}
}
