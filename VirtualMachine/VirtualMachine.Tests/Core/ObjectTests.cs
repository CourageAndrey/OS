using NUnit.Framework;

using VirtualMachine.Core;

namespace VirtualMachine.Tests.Core
{
	public class ObjectTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void Test1()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			var freeAddress = memory.GetNextFreeAddress();

			// act
			var @object = new Object(memory);

			// assert
			Assert.AreSame(memory, @object._memory);
			Assert.AreEqual(freeAddress, @object._memoryAddress);
			Assert.Greater(memory.GetNextFreeAddress(), freeAddress);
		}
	}
}