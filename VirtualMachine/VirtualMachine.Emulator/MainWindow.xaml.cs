using System.Linq;

using VirtualMachine.Core;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Emulator
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			var memory = new Memory();
			memory.Serialize();

			listBoxTypes.ItemsSource = memory.Types.Select(t => $"[{t.Value.Address:X4}] : {t.Key}");
			textBoxDump.Text = memory.GetDump();
		}
	}
}
