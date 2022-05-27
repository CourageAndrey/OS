using System;

namespace VirtualMachine.Core
{
	public class Integer : Object, IEquatable<Integer>
	{
		#region Properties

		internal readonly int Value;

		#endregion

		#region Conctructors

		public Integer(int value)
		{
			Value = value;
		}

		public static Integer Create(int value)
		{
			var result = new Integer(value);
			result.CallConstructor(IntegerDataType);
			return result;
		}

		#endregion

		#region Metadata

		public static readonly DataType IntegerDataType = new DataType();

		#endregion

		#region Implementation of IEquatable

		public bool Equals(Integer other)
		{
			return Value.Equals(other?.Value);
		}

		public override bool Equals(object other)
		{
			return Equals(other as Integer);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		#endregion
	}
}
