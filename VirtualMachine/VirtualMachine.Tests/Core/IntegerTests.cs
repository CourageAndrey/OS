using System.Linq;

using NUnit.Framework;

using VirtualMachine.Core;

namespace VirtualMachine.Tests.Core
{
	public class IntegerTests
	{
		[Test]
		public void GivenMemory_WhenCreateNewInteger_ThenCreateZero()
		{
			// arrange
			var memory = new Memory();
			memory.Serialize();

			var freeAddress = memory.NextFreeAddress;

			// act
			var integer = new Integer();
			memory.Allocate(integer);

			// assert
			Assert.AreSame(memory, integer.Memory);
			Assert.AreSame(memory.IntegerDataType, integer.GetDataType());
			Assert.AreEqual(0, integer.Value);
			Assert.AreEqual(freeAddress, integer.Address);
			Assert.Greater(memory.NextFreeAddress, freeAddress);
		}

		[Test]
		public void GivenInteger_WhenCheckToString_ThenReturnValue()
		{
			// arrange
			var memory = new Memory();
			memory.Serialize();

			var integer = new Integer();
			memory.Allocate(integer);

			const int value = 123;

			// act
			integer.Value = value;

			// assert
			Assert.AreEqual(value.ToString(), integer.ToString());
		}

		[Test]
		public void GivenDetached_WhenSetValue_ThenUpdate()
		{
			// arrange
			const int value = 123;

			// act
			var integer = new Integer { Value = value };

			// assert
			Assert.AreEqual(value.ToString(), integer.ToString());
		}

		[Test]
		public void WhenSerialize_ThenReturnNumber()
		{
			// arrange
			var memory = new Memory();

			const int value = 123;
			var integer = new Integer { Value = value };

			// act
			var bytes = integer.Serialize(memory, 1);

			// assert
			Assert.AreEqual(value, bytes.Single());
		}
	}
}