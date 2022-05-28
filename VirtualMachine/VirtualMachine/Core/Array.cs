using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class Array : ClassInstance
	{
		#region Properties

		protected internal const MemoryOffset FieldOffsetLength = FieldsCountOfObjectClass + 0;

		protected internal const MemoryOffset FieldsCountOfArrayClass = 1;
		protected internal const MemoryOffset TotalFieldsCountOfArrayClass = TotalFieldsCountOfObjectClass + FieldsCountOfArrayClass;

		public Integer Length
		{ get { return GetFieldValue<Integer>(FieldOffsetLength); } }

		public Object this[MemoryAddress index]
		{
			get
			{
#warning No structs are processed here! Need to make array typed.
				MemoryAddress pointer = TotalFieldsCountOfArrayClass + index;
				return GetFieldValue<Object>(pointer);
			}
		}

		#endregion

		#region Conctructors

		public Array(Memory memory, MemoryAddress memoryAddress)
			: base(memory, memoryAddress)
		{ }

		#endregion
	}
}
