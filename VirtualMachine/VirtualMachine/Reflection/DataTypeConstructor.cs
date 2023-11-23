﻿using VirtualMachine.Core;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Reflection
{
	public class DataTypeConstructor : DataTypeMember
	{
		#region Properties



		#endregion

		#region Constructors

		internal DataTypeConstructor(string name)
			: base(name)
		{ }

		#endregion
	}
}
