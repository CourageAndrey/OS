namespace VirtualMachine.Core
{
	public class Exception : Object
	{
		public String Message
		{ get { return _message; } }

		private readonly String _message;

		internal Exception(String message)
			: base(Class.ExceptionClass)
		{
			_message = message;
		}
	}
}
