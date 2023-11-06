using VirtualMachine.Core;

namespace VirtualMachine.Tests
{
	internal class UnknownStructure : Structure
	{
		public UnknownStructure(Memory memory, int memoryAddress)
			: base(memory, memoryAddress, null)
		{ }

		public override int GetDataSize()
		{
			return 0;
		}
	}
}
