using NUnit.Framework;

using VirtualMachine.Core;

using MemoryWord = System.UInt64;

namespace VirtualMachine.Tests.Core
{
	public class IntegerTests
	{
		[Test]
		public void GivenMemory_WhenCreateNewInteger_ThenCreateZero()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			var freeAddress = memory.GetNextFreeAddress();

			// act
			var integer = new Integer(memory, freeAddress);

			// assert
			Assert.AreSame(memory, integer._memory);
			Assert.AreSame(memory.IntegerDataType, integer.GetDataType());
			Assert.AreEqual(1, integer.GetReferencedDataSize());
			Assert.AreEqual(1, integer.GetVariableSize());
			Assert.AreEqual(0, integer.Value);
			Assert.AreEqual(freeAddress, integer._memoryAddress);
			Assert.Greater(memory.GetNextFreeAddress(), freeAddress);
		}

		[Test]
		public void GivenInteger_WhenCheckToString_ThenReturnValue()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);
			const MemoryWord value = 123;

			// act
			var integer = new Integer(memory, value);

			// assert
			Assert.AreEqual(value.ToString(), integer.ToString());
		}
	}
}