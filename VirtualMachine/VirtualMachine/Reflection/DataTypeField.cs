using VirtualMachine.Core;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Reflection
{
	public class DataTypeField : DataTypeMember
	{
		#region Properties



		#endregion

		#region Constructors

		internal DataTypeField(string name)
		{
			Tag = name;
		}

		#endregion
	}
}
