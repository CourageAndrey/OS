using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public sealed class Integer : ValueObject
	{
		#region Properties

		internal const MemoryOffset IntegerFieldsCount = 1;
		internal const MemoryOffset FieldOffsetValue = 0;

		private ulong _value;

		public ulong Value
		{
			get
			{
				return IsInMemory
					? Memory.Cells[Address + FieldOffsetValue]
					: _value;
			}
			set
			{
				if (IsInMemory)
				{
					Memory.Cells[Address + FieldOffsetValue] = value;
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
			return Memory.IntegerDataType;
		}

		internal sealed override System.Collections.Generic.List<MemoryWord> Serialize(Memory memory, MemoryAddress address)
		{
			return new System.Collections.Generic.List<MemoryWord> { _value };
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
