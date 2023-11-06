using VirtualMachine.Core;

namespace VirtualMachine.Tests
{
	internal class UnknownStructure : Structure
	{
		public UnknownStructure(Memory memory, int memoryAddress)
			: base(memory, memoryAddress, null)
		{ }

		public override int GetReferencedDataSize()
		{
			return 0;
		}
	}
}
