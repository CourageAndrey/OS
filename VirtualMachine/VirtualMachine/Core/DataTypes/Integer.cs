using VirtualMachine.Core.Reflection;

namespace VirtualMachine.Core.DataTypes
{
	public class Integer : Object
	{
		public static readonly Integer Zero = new Integer(0);

		private readonly int _value;

		internal Integer(int value)
			: base(Class.IntegerClass)
		{
			_value = value;
		}

		public override String ToString()
		{
			return new String(string.Empty + _value);
		}

		public int DebugValue
		{ get { return _value; } }
	}
}
