using System.Collections.Generic;

using VirtualMachine.Core.DataTypes;
using VirtualMachine.Core.Reflection;

namespace VirtualMachine.Core
{
	public abstract class Collection : Object
	{
		public abstract Integer Count
		{ get; }

		internal Collection()
			: base(Class.CollectionClass)
		{ }

		public override String ToString()
		{
			return new String("Collection[").Concat(Count.ToString()).Concat(new String("]"));
		}
	}

	public class Collection<T> : Collection, IEnumerable<T>
		where T : Object
	{
		public override Integer Count
		{ get { return new Integer(_items.Count); } }

		private readonly ICollection<T> _items; 

		internal Collection(ICollection<T> items)
		{
			_items = items;
		}

		public Collection()
			: this(new T[0])
		{ }

		public IEnumerator<T> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
