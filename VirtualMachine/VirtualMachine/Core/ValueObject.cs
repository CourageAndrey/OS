using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public abstract class ValueObject : MemoryObject
	{
		public sealed override MemoryWord ReferencedDataSize
		{ get { return 0; } }
	}
}
