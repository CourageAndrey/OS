using VirtualMachine.Core.DataTypes;

namespace VirtualMachine
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			Content = new String("Hello, world!").DebugValue;
		}
	}
}
