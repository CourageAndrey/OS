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

			var text = new System.Text.StringBuilder();
			var array = memory.ObjectDataType.GetDataType().Fields;

			for (MemoryWord f = 0; f < array.Length; f++)
			{
				text.AppendLine(array[(int) f].Tag);
			}

			var @int = new Integer();
			@int.Serialize(memory, memory.NextFreeAddress);
			@int.Value = 123;
			var @char = new Char();
			@char.Serialize(memory, memory.NextFreeAddress);
			@char.Value = 'A';
			text.AppendLine(@int.ToString());
			text.AppendLine(@char.ToString());

			Content = text;
		}
	}
}
