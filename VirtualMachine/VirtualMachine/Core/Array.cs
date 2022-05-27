using VirtualMachine.Reflection;

namespace VirtualMachine.Core
{
	public class Array : Object
	{
		#region Properties

		public Integer Length
		{
			get { return (Integer) _data[ObjectDataType.Fields.Count + 0]; }
			internal set { _data[ObjectDataType.Fields.Count + 0] = value; }
		}

		internal readonly Object[] Items;

		#endregion

		#region Conctructors

		public Array(Integer length)
		{
			Items = new Object[length.Value];
		}

		public static Array Create(Integer length)
		{
			var result = new Array(length);
			result.CallConstructor(ArrayDataType);
			result.Length = length;
			return result;
		}

		#endregion

		#region Metadata

		public static readonly DataType ArrayDataType = new DataType();

		internal static DataTypeField FieldLength;

		#endregion
	}
}
