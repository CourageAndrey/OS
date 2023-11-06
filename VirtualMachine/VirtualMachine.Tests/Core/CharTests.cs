using NUnit.Framework;

using VirtualMachine.Core;

namespace VirtualMachine.Tests.Core
{
	public class CharTests
	{
		[Test]
		public void GivenMemory_WhenCreateNewChar_ThenCreateEmptyChar()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			var freeAddress = memory.GetNextFreeAddress();

			// act
			var @char = new Char(memory, freeAddress);

			// assert
			Assert.AreSame(memory, @char._memory);
			Assert.AreSame(memory.CharDataType, @char.GetDataType());
			Assert.AreEqual(1, @char.GetDataSize());
			Assert.AreEqual(1, @char.GetVariableSize());
			Assert.AreEqual(0, @char.Value);
			Assert.AreEqual(freeAddress, @char._memoryAddress);
			Assert.Greater(memory.GetNextFreeAddress(), freeAddress);
		}

		[Test]
		public void GivenChar_WhenCheckToString_ThenReturnValue()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);
			const char value = '@';

			// act
			var @char = new Char(memory, value);

			// assert
			Assert.AreEqual(value.ToString(), @char.ToString());
		}
	}
}