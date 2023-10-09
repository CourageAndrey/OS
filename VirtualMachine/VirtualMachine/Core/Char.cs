using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class Char : Structure
	{
		#region Properties

		public MemoryWord Value
		{
			get { return _memory.Cells[_memoryAddress]; }
			set { _memory.Cells[_memoryAddress] = value; }
		}

		public override MemoryOffset GetDataSize()
		{
			return 1; // one char
		}

		#endregion

		#region Constructors

		public Char(Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress, memory.IntegerDataType)
		{ }

		#endregion

		public override string ToString()
		{
			return ((char) Value).ToString();
		}
	}
}
