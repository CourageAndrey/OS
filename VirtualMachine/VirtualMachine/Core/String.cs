using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class String : Array<Char>
	{
		#region Conctructors

		public String(Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress, 1) // Char."DataSize"
		{ }

		#endregion

		public override string ToString()
		{
			var result = string.Empty;
			for (MemoryAddress c = 0; c < (MemoryAddress) Length.Value; c++)
			{
				result += this[c].ToString();
			}
			return result;
		}
	}
}
