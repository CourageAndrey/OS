namespace VirtualMachine
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			Content = Core.Object.Create<Core.Object>(Core.Object.ObjectDataType).DataType;
		}
	}
}
