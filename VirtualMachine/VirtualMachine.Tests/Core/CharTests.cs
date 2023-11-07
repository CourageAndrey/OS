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
			var memory = new Memory();
			memory.Serialize();

			var freeAddress = memory.NextFreeAddress;

			// act
			var @char = new Char();
			memory.Allocate(@char);

			// assert
			Assert.AreSame(memory, @char.Memory);
			Assert.AreSame(memory.CharDataType, @char.GetDataType());
			Assert.AreEqual(0, @char.Value);
			Assert.AreEqual(freeAddress, @char.Address);
			Assert.Greater(memory.NextFreeAddress, freeAddress);
		}

		[Test]
		public void GivenChar_WhenCheckToString_ThenReturnValue()
		{
			// arrange
			var memory = new Memory();
			memory.Serialize();

			var @char = new Char();
			memory.Allocate(@char);

			const char value = '@';

			// act
			@char.Value = value;

			// assert
			Assert.AreEqual(value.ToString(), @char.ToString());
		}
	}
}