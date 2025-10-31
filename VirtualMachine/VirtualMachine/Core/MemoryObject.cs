using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public abstract class MemoryObject
	{
		#region Properties

		public Memory Memory
		{ get; internal set; }

		public MemoryAddress Address
		{ get; internal set; }

		public virtual MemoryWord VariableSize
		{ get { return 1; } }

		public abstract MemoryWord ReferencedDataSize
		{ get; }

		public abstract DataType GetDataType();

		internal string Tag;

		protected internal bool IsInMemory
		{ get { return Memory != null; } }

		#endregion

		protected internal void ValidateMemory(MemoryAddress offset)
		{
			if (Memory == null)
			{
				throw new System.InvalidOperationException("Object is not in memory.");
			}

			if (Address <= 0)
			{
				throw new System.InvalidOperationException("Object has invalid address.");
			}

			if (offset < 0)
			{
				throw new System.ArgumentException("Field offset has to be positive.");
			}
		}

		protected internal ObjectT GetFieldValue<ObjectT>(MemoryAddress fieldOffset)
			where ObjectT : MemoryObject
		{
			ValidateMemory(fieldOffset);

			if (typeof(ReferencedObject).IsAssignableFrom(typeof(ObjectT)))
			{
				var address = (MemoryAddress) Memory.Cells[Address + fieldOffset];
				return address > 0
					? (ObjectT) Memory.Objects[address]
					: null;
			}

			else if (typeof(ObjectT) == typeof(Integer))
			{
				return new Integer
				{
					Memory = Memory,
					Address = Address + fieldOffset,
				} as ObjectT;
			}
			else if (typeof(ObjectT) == typeof(Char))
			{
				return new Char
				{
					Memory = Memory,
					Address = Address + fieldOffset,
				} as ObjectT;
			}

			else
			{
				throw new System.NotSupportedException("Unknown object type.");
			}
		}

		protected internal void SetFieldValue<ObjectT>(MemoryAddress fieldOffset, ObjectT value)
			where ObjectT : MemoryObject
		{
			ValidateMemory(fieldOffset);

			if (typeof(ReferencedObject).IsAssignableFrom(typeof(ObjectT)))
			{
				var refObject = value as ReferencedObject;
				if (refObject.IsInMemory)
				{
					Memory.Cells[Address + fieldOffset] = (MemoryWord) refObject.Address;
				}
				else
				{
					throw new System.InvalidOperationException("Value objct is not in memory.");
				}
			}

			else if (typeof(ObjectT) == typeof(Integer))
			{
				Memory.Cells[Address + fieldOffset] = (value as Integer).Value;
			}
			else if (typeof(ObjectT) == typeof(Char))
			{
				Memory.Cells[Address + fieldOffset] = (value as Char).Value;
			}

			else
			{
				throw new System.NotSupportedException("Unknown object type.");
			}
		}

		internal abstract System.Collections.Generic.List<MemoryWord> Serialize(Memory memory, MemoryAddress address);

		public override string ToString()
		{
			return Tag ?? base.ToString();
		}
	}
}
