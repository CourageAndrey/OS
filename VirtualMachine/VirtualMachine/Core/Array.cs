using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public abstract class Array : ReferencedObject, System.Collections.IEnumerable
	{
		#region Properties

		protected const MemoryOffset ArrayFieldsCount = 1;
		protected const MemoryOffset ArrayFieldsTotalCount = ObjectFieldsCount + ArrayFieldsCount;
		protected const MemoryOffset FieldOffsetLength = ObjectFieldsCount + 0;

		private readonly System.Array _items;

		public ulong Length
		{
			get
			{
				return IsInMemory
					? Memory.Cells[Address + FieldOffsetLength]
					: (ulong) _items.Length;
			}
		}

		protected readonly MemoryWord ItemSize;

		public override MemoryWord ReferencedDataSize
		{ get { return base.ReferencedDataSize + Length * ItemSize; } }

		#endregion

		#region Constructors

		protected Array(System.Array items)
			: base()
		{
			_items = items;
			ItemSize = 1;
		}

		#endregion

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _items.GetEnumerator();
		}
	}

	public class Array<ItemT> : Array, System.Collections.Generic.IEnumerable<ItemT>
		where ItemT : MemoryObject
	{
		#region Properties

		private readonly ItemT[] _items;

		public ItemT this[MemoryAddress index]
		{
			get
			{
				if (IsInMemory)
				{
					return GetFieldValue<ItemT>(ArrayFieldsTotalCount + index * (MemoryAddress) ItemSize);
				}
				else
				{
					return _items[index];
				}
			}
			set
			{
				if (IsInMemory)
				{
#warning Implement array member setter!
					throw new System.NotImplementedException();
				}
				else
				{
					_items[index] = value;
				}
			}
		}

		#endregion

		#region Constructors

		public Array(ItemT[] items)
			: base(items)
		{
			_items = items;
			// change ItemSize in case of larger size structures
		}

		#endregion

		public System.Collections.Generic.IEnumerator<ItemT> GetEnumerator()
		{
			return (_items as System.Collections.Generic.IEnumerable<ItemT>).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected override System.Collections.Generic.List<MemoryWord> SerializeReferencedData(Memory memory, MemoryAddress address)
		{
			var data = base.SerializeReferencedData(memory, address);
			data.Add(Length);

#warning Instead of this shit struct- and reference-types must be serialized separately
			if (typeof(ReferencedObject).IsAssignableFrom(typeof(ItemT)))
			{
				data.AddRange(new MemoryWord[Length]);

				var itemAddress = address + ArrayFieldsTotalCount;
				for (var i = 0; i < _items.Length; i++)
				{
					var item = _items[i];

					var itemData = item.Serialize(memory, itemAddress);
					data.AddRange(itemData);

					data[ArrayFieldsTotalCount + i] = (MemoryWord) itemAddress;
					itemAddress += itemData.Count;
				}
			}
			else
			{
				for (int i = 0; i < _items.Length; i++)
				{
					data.AddRange(_items[i].Serialize(memory, address + ArrayFieldsTotalCount + i * (int) ItemSize));
				}
			}

			return data;
		}
	}
}
