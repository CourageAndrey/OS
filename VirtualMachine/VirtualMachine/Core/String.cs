namespace VirtualMachine.Core
{
	public class String : Object
	{
		public static readonly String Empty = new String(string.Empty);

		public Integer Length
		{ get { return new Integer(_value.Length); } }

		private readonly string _value;

		internal String(string value)
			: base(Class.StringClass)
		{
			_value = value;
		}

		public override String ToString()
		{
			return this;
		}

#warning Add all corresponding method infos to class description

		public String Concat(String other)
		{
			return new String(_value + other._value);
		}

		public string DebugValue
		{ get { return _value; } }
	}
}
