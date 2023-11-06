﻿using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public abstract class Array : ClassInstance
	{
		#region Properties

		protected internal const MemoryOffset FieldOffsetLength = FieldsCountOfObjectClass + 0;

		protected internal const MemoryOffset FieldsCountOfArrayClass = 1;
		protected internal const MemoryOffset TotalFieldsCountOfArrayClass = TotalFieldsCountOfObjectClass + FieldsCountOfArrayClass;

		public Integer Length
		{ get { return GetFieldValue<Integer>(FieldOffsetLength); } }

		#endregion

		#region Constructors

		protected Array(Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{ }

		#endregion
	}

	public class Array<ItemT> : Array
		where ItemT : Object
	{
		#region Properties

		private readonly MemoryOffset _itemDataSize;

		public override MemoryOffset GetReferencedDataSize()
		{
			return TotalFieldsCountOfArrayClass + ((MemoryAddress) Length.Value) * _itemDataSize;
		}

		public ItemT this[MemoryAddress index]
		{ get { return _memory.GetObject<ItemT>(_memoryAddress + TotalFieldsCountOfArrayClass + index * _itemDataSize); } }

		#endregion

		#region Constructors

		public Array(Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{
			_itemDataSize = memory.GetDataType<ItemT>().GetVariableSize();
		}

		#endregion
	}
}
