using NUnit.Framework;

using VirtualMachine.Core;

namespace VirtualMachine.Tests.Core
{
	public class ObjectTests
	{
		[Test]
		public void GivenMemory_WhenCreateNewObject_ThenCreateNewEmptyObject()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			var freeAddress = memory.GetNextFreeAddress();

			// act
			var @object = new ClassInstance(memory, memory.ObjectDataType);

			// assert
			Assert.AreSame(memory, @object._memory);
			Assert.AreSame(memory.ObjectDataType, @object.GetDataType());
			Assert.AreEqual(1, @object.GetDataSize());
			Assert.AreEqual(1, @object.GetVariableSize());
			Assert.AreEqual(freeAddress, @object._memoryAddress);
			Assert.Greater(memory.GetNextFreeAddress(), freeAddress);
		}
	}
}