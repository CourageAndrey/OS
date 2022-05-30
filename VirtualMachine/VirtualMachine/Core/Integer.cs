using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class Integer : Structure
	{
		#region Properties

		public MemoryWord Value
		{
			get { return _memory.Cells[_memoryAddress]; }
			set { _memory.Cells[_memoryAddress] = value; }
		}

		public override MemoryOffset DataSize
		{ get { return 1; } } // one memory word of data

		#endregion

		#region Conctructors

		public Integer(Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress, memory.IntegerDataType)
		{ }

		#endregion

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
