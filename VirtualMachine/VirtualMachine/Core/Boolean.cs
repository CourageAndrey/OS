namespace VirtualMachine.Core
{
	public class Boolean : Object
	{
		public static readonly Boolean True = new Boolean();
		public static readonly Boolean False = new Boolean();

		private Boolean()
			: base(Class.BooleanClass)
		{ }

		internal static Boolean FromSystem(bool value)
		{
			return value ? True : False;
		}

		public override String ToString()
		{
			return this == True ? new String("+") : new String("-");
		}

		public bool Debug
		{ get { return this == True; } }
	}
}
