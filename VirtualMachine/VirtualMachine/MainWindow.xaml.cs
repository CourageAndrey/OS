namespace VirtualMachine
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			Content = new Core.String("Hello, world!").DebugValue;
		}
	}
}
