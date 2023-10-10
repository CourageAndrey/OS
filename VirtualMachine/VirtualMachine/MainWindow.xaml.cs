using VirtualMachine.Core;
using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			var data = Core.Environment.LoadSample();
			var memory = new Core.Memory(data.Item1, data.Item2);

			var text = new System.Text.StringBuilder();
			var array = memory.ObjectDataType.GetDataType().Fields;

			for (MemoryWord f = 0; f < array.Length.Value; f++)
			{
				text.AppendLine(array[(int) f].Tag);
			}

			text.AppendLine(new Integer(memory, (MemoryWord) 123).ToString());
			text.AppendLine(new Char(memory, 'A').ToString());

			Content = text;
		}
	}
}
