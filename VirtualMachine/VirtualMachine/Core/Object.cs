using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public abstract class Object
	{
		#region Properties

		protected readonly Memory _memory;
		protected readonly MemoryAddress _memoryAddress;

		public abstract DataType DataType
		{ get; }

		internal string Tag;

		#endregion

		#region Conctructors

		protected Object(Memory memory, MemoryAddress memoryAddress)
		{
			_memory = memory;
			_memoryAddress = memoryAddress;
		}

		#endregion

		protected ObjectT GetFieldValue<ObjectT>(MemoryOffset field)
			where ObjectT : Object
		{
			MemoryAddress fieldCellAddress = _memoryAddress + field;

			if (typeof(Structure).IsAssignableFrom(typeof(ObjectT)))
			{
#warning Wrong implementation of right approach.
				if (typeof(ObjectT) == typeof(Integer))
				{
					return new Integer(_memory, fieldCellAddress) as ObjectT;
				}
				else
				{
					throw new System.NotSupportedException();
				}
			}
			else
			{
				MemoryAddress fieldValue = (MemoryAddress) _memory.Cells[fieldCellAddress];
				return _memory.GetObject<ObjectT>(fieldValue);
			}
		}

		public override string ToString()
		{
			return Tag ?? base.ToString();
		}
	}

	public abstract class ClassInstance : Object
	{
		#region Properties

		protected internal const MemoryOffset FieldOffsetDataType = 0;

		protected internal const MemoryOffset FieldsCountOfObjectClass = 1;
		protected internal const MemoryOffset TotalFieldsCountOfObjectClass = FieldsCountOfObjectClass;

		public override DataType DataType
		{ get { return GetFieldValue<DataType>(FieldOffsetDataType); } }

		#endregion

		#region Conctructors

		protected ClassInstance(Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{ }

		#endregion
	}

	public abstract class Structure : Object
	{
		public override DataType DataType
		{ get { return _dataType; } }

		private readonly DataType _dataType;

		protected Structure(Memory memory, MemoryAddress memoryAddress, DataType dataType)
			: base(memory, memoryAddress)
		{
			_dataType = dataType;
		}
	}

	/*public abstract class Interface : Object
	{

	}*/
}
