using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public sealed class Bool : ValueObject
	{
		#region Properties

		internal const MemoryOffset BoolFieldsCount = 1;
		internal const MemoryOffset FieldOffsetValue = 0;

		private bool _value;

		public bool Value
		{
			get
			{
				return IsInMemory
					? Memory.Cells[Address + FieldOffsetValue] != 0
					: _value;
			}
			set
			{
				if (IsInMemory)
				{
					Memory.Cells[Address + FieldOffsetValue] = value ? (MemoryWord) 1 : 0;
				}
				else
				{
					_value = value;
				}
			}
		}

		#endregion

		#region Constructors



		#endregion

		public override DataType GetDataType()
		{
			return Memory.BoolDataType;
		}

		internal sealed override System.Collections.Generic.List<MemoryWord> Serialize(Memory memory, MemoryAddress address)
		{
			return new System.Collections.Generic.List<MemoryWord> { _value ? (MemoryWord) 1 : 0 };
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
