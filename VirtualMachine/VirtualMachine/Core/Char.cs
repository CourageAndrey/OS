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

		public Char(Memory memory, MemoryWord value = 0)
			: base(memory)
		{
			Value = value;
		}

		public Char(Memory memory, char value = '\0')
			: this(memory, (MemoryWord) value)
		{ }

		#endregion

		public override string ToString()
		{
			return ((char) Value).ToString();
		}
	}
}
