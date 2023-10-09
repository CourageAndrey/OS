using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class Object
	{
		#region Properties

		protected readonly Memory _memory;
		protected readonly MemoryAddress _memoryAddress;

		public virtual DataType GetDataType()
		{
			return _memory.ObjectDataType;
		}

		public virtual MemoryOffset GetVariableSize()
		{
			return 1; // one memory word-length pointer
		}

		public virtual MemoryOffset GetDataSize()
		{
			return 0;
		}

		internal string Tag;

		#endregion

		#region Constructors

		protected Object(Memory memory, MemoryAddress memoryAddress)
		{
			_memory = memory;
			_memoryAddress = memoryAddress;
		}

		#endregion

		protected ObjectT GetFieldValue<ObjectT>(MemoryOffset field)
			where ObjectT : Object
		{
			return _memory.GetObject<ObjectT>(_memoryAddress + field);
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

		public override DataType GetDataType()
		{
			return GetFieldValue<DataType>(FieldOffsetDataType);
		}

		public override MemoryOffset GetVariableSize()
		{
			return 1; // one memory word-length pointer
		}

		#endregion

		#region Constructors

		protected ClassInstance(Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{ }

		#endregion
	}

	public abstract class Structure : Object
	{
		public override DataType GetDataType()
		{
			return _dataType;
		}

		private readonly DataType _dataType;

		public override MemoryOffset GetVariableSize()
		{
			return GetDataSize(); // because structures are stored by values
		}

		protected Structure(Memory memory, MemoryAddress memoryAddress, DataType dataType)
			: base(memory, memoryAddress)
		{
			_dataType = dataType;
		}

		public static StructT TypeCast<StructT>(Memory memory, MemoryAddress memoryAddress)
			where StructT : Structure
		{
			if (typeof(StructT) == typeof(Integer))
			{
				return new Integer(memory, memoryAddress) as StructT;
			}
			else
			{
				throw new System.InvalidCastException();
			}
		}
	}

	/*public abstract class Interface : Object
	{

	}*/
}
