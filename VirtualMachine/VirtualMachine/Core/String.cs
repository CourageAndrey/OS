using System.Linq;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class String : Array<Char>
	{
		#region Constructors

		public String(string value)
			: base(value.ToCharArray().Select(c => new Char { Value = c }).ToArray())
		{ }

		#endregion

		public override string ToString()
		{
			var result = string.Empty;
			for (MemoryAddress c = 0; c < (MemoryAddress) Length; c++)
			{
				result += this[c].ToString();
			}
			return result;
		}
	}
}
