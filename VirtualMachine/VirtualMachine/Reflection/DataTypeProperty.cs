﻿using VirtualMachine.Core;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Reflection
{
	public class DataTypeProperty : DataTypeMember
	{
		#region Properties



		#endregion

		#region Constructors

		internal DataTypeProperty(string name)
			: base(name)
		{ }

		#endregion
	}
}
