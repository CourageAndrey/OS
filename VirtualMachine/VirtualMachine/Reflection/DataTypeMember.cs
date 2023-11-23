using VirtualMachine.Core;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Reflection
{
	public abstract class DataTypeMember : ReferencedObject
	{
		#region Properties

		protected internal const MemoryOffset DataTypeMemberFieldsCount = 1;
		protected internal const MemoryOffset DataTypeMemberFieldsTotalCount = ObjectFieldsTotalCount + DataTypeMemberFieldsCount;
		protected internal const MemoryOffset FieldOffsetName = ObjectFieldsTotalCount + 0;

		private readonly String _name;

		public String Name
		{
			get
			{
				return IsInMemory
					? GetFieldValue<String>(FieldOffsetName)
					: _name;
			}
		}

		#endregion

		#region Constructors

		protected DataTypeMember(string name)
		{
			_name = new String(Tag = name);
		}

		#endregion

		protected override System.Collections.Generic.List<MemoryWord> SerializeReferencedData(Memory memory, MemoryAddress address)
		{
			var data = base.SerializeReferencedData(memory, address);

			var nameData = Name.Serialize(memory, address + DataTypeMemberFieldsTotalCount);

			data.Add((MemoryWord) Name.Address);

			data.AddRange(nameData);

			return data;
		}

		public override string ToString()
		{
			return Name.ToString();
		}
	}
}
