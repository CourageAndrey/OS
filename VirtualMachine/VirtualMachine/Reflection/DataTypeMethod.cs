using VirtualMachine.Core;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Reflection
{
	public class DataTypeMethod : DataTypeMember
	{
		#region Properties

		private readonly MemoryWord[] _code;

		public override MemoryWord ReferencedDataSize
		{ get { return base.ReferencedDataSize + (MemoryWord) _code.Length; } }

		#endregion

		#region Constructors

		internal DataTypeMethod(string name, MemoryWord[] code)
			: base(name)
		{
			if (code == null) throw new System.ArgumentNullException(nameof(code));
			if (code.Length == 0) throw new System.ArgumentException("Code length must be positive.", nameof(code));

			_code = code;
		}

		#endregion

		protected override System.Collections.Generic.List<MemoryWord> SerializeReferencedData(Memory memory, MemoryAddress address)
		{
			var data = base.SerializeReferencedData(memory, address);

			data.AddRange(_code);

			return data;
		}
	}
}
