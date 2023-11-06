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
			Assert.AreEqual(1, @object.GetReferencedDataSize());
			Assert.AreEqual(1, @object.GetVariableSize());
			Assert.AreEqual(freeAddress, @object._memoryAddress);
			Assert.Greater(memory.GetNextFreeAddress(), freeAddress);
		}

		[Test]
		public void GivenObject_WhenCheckToString_ThenReturnTag()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);
			var @object = new ClassInstance(memory, memory.ObjectDataType);

			// act & assert
			Assert.IsNull(@object.Tag);
			Assert.AreEqual(@object.GetType().FullName, @object.ToString());
			@object.Tag = "Test";
			Assert.AreEqual("Test", @object.Tag);
			Assert.AreEqual("Test", @object.ToString());
		}

		[Test]
		public void GivenChar_WhenTypeCast_ThenCast()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			var freeAddress = memory.GetNextFreeAddress();
			memory.Cells[freeAddress] = 123;

			// act
			var @char = Structure.TypeCast<Char>(memory, freeAddress);

			// assert
			Assert.AreEqual(123, @char.Value);
		}

		[Test]
		public void GivenInteger_WhenTypeCast_ThenCast()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			var freeAddress = memory.GetNextFreeAddress();
			memory.Cells[freeAddress] = 123;

			// act
			var integer = Structure.TypeCast<Integer>(memory, freeAddress);

			// assert
			Assert.AreEqual(123, integer.Value);
		}

		[Test]
		public void GivenUnknown_WhenTryToTypeCast_ThenFail()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			var freeAddress = memory.GetNextFreeAddress();
			var structure = new UnknownStructure(memory, freeAddress);

			// act && assert
			Assert.Throws<System.InvalidCastException>(() => Structure.TypeCast<UnknownStructure>(memory, structure._memoryAddress));
		}
	}
}