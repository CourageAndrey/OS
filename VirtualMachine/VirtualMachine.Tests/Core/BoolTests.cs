using NUnit.Framework;

using VirtualMachine.Core;

namespace VirtualMachine.Tests.Core
{
	public class BoolTests
	{
		[Test]
		public void GivenMemory_WhenCreateNewBool_ThenCreateFalse()
		{
			// arrange
			var memory = new Memory();
			memory.Serialize();

			var freeAddress = memory.NextFreeAddress;

			// act
			var @bool = new Bool();
			memory.Allocate(@bool);

			// assert
			Assert.AreSame(memory, @bool.Memory);
			Assert.AreSame(memory.BoolDataType, @bool.GetDataType());
			Assert.AreEqual(false, @bool.Value);
			Assert.AreEqual(freeAddress, @bool.Address);
			Assert.Greater(memory.NextFreeAddress, freeAddress);
		}

		[Test]
		public void GivenBool_WhenCheckToString_ThenReturnValue()
		{
			// arrange
			var memory = new Memory();
			memory.Serialize();

			var @bool = new Bool();
			memory.Allocate(@bool);

			const bool value = true;

			// act
			@bool.Value = value;

			// assert
			Assert.AreEqual(value.ToString(), @bool.ToString());
		}
	}
}