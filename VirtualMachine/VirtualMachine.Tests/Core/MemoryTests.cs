using System.Linq;

using NUnit.Framework;

using VirtualMachine.Core;
using VirtualMachine.Reflection;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Tests.Core
{
	public class MemoryTests
	{
		[Test]
		public void GivenNoCells_WhenTryToCreateMemory_ThenFail()
		{
			// act & assert
			Assert.Throws<System.ArgumentNullException>(() => new Memory(null, 0));
		}

		[Test]
		public void GivenSmallMemory_WhenTryToCreateMemory_ThenFail()
		{
			// act & assert
			Assert.Throws<System.ArgumentException>(() => new Memory(new MemoryWord[Memory.RequiredSize / 10], 0));
		}

		[Test]
		public void GivenNonEmptyZeroPointer_WhenTryToCreateMemory_ThenFail()
		{
			// arrange
			var cells = new MemoryWord[Memory.RequiredSize];
			cells[0] = 1;

			// act & assert
			Assert.Throws<System.ArgumentException>(() => new Memory(cells, 0));
		}

		[Test]
		public void GivenInvalidObjectTypeAddress_WhenTryToCreateMemory_ThenFail()
		{
			// arrange
			var cells = new MemoryWord[Memory.RequiredSize];

			// act & assert
			Assert.Throws<System.ArgumentException>(() => new Memory(cells, -1));
			Assert.Throws<System.ArgumentException>(() => new Memory(cells, cells.Length));
			Assert.Throws<System.ArgumentException>(() => new Memory(cells, cells.Length + 1));
			Assert.Throws<System.NullReferenceException>(() => new Memory(cells, 0));
		}

		[Test]
		public void GivenMemory_WhenCreateWithTypes_ThenAllInitialized()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			// assert
			Assert.GreaterOrEqual(memory.Size, Memory.RequiredSize);
			Assert.AreEqual(memory.Size, memory.Cells.Length);

			var dataTypes = memory.Objects.Values.OfType<DataType>().ToHashSet();
			Assert.AreEqual(12, dataTypes.Count);
			Assert.IsTrue(dataTypes.Contains(memory.ObjectDataType));
			Assert.IsTrue(dataTypes.Contains(memory.DataTypeDataType));
			Assert.IsTrue(dataTypes.Contains(memory.DataTypeMemberDataType));
			Assert.IsTrue(dataTypes.Contains(memory.DataTypeFieldDataType));
			Assert.IsTrue(dataTypes.Contains(memory.DataTypeMethodDataType));
			Assert.IsTrue(dataTypes.Contains(memory.DataTypePropertyDataType));
			Assert.IsTrue(dataTypes.Contains(memory.DataTypeEventDataType));
			Assert.IsTrue(dataTypes.Contains(memory.DataTypeConstructorDataType));
			Assert.IsTrue(dataTypes.Contains(memory.IntegerDataType));
			Assert.IsTrue(dataTypes.Contains(memory.ArrayDataType));
			Assert.IsTrue(dataTypes.Contains(memory.CharDataType));
			Assert.IsTrue(dataTypes.Contains(memory.StringDataType));
		}

		[Test]
		public void GivenInvalidPointer_WhenGetObject_ThenFail()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			// act & assert
			Assert.Throws<System.Exception>(() => memory.GetObject<DataType>(-1));
			Assert.Throws<System.Exception>(() => memory.GetObject<DataType>(memory.Size));
			Assert.Throws<System.Exception>(() => memory.GetObject<DataType>(memory.Size + 1));
		}

		[Test]
		public void GivenStructures_WhenGetObject_ThenReturnThem()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			// act & assert
			var @int = memory.GetObject<Integer>(1);
			Assert.AreEqual(11, @int.Value);
			var @char = memory.GetObject<Char>(1);
			Assert.AreEqual(11, @char.Value);
			Assert.Throws<System.NotSupportedException>(() => memory.GetObject<UnknownStructure>(1));
		}

		private class UnknownStructure : Structure
		{
			public UnknownStructure(Memory memory, int memoryAddress, DataType dataType)
				: base(memory, memoryAddress, null)
			{ }

			public override int GetDataSize()
			{
				return 0;
			}
		}

		[Test]
		public void GivenZeroPointer_WhenGetObject_ThenReturnNull()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			// act & assert
			Assert.IsNull(memory.GetObject<DataType>(0));
		}

		[Test]
		public void GivenWrongType_WhenGetObject_ThenFail()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			// act & assert
			Assert.Throws<System.InvalidCastException>(() => memory.GetObject<DataTypeField>(memory.ObjectDataType._memoryAddress));
		}

		[Test]
		public void GivenWrongAddress_WhenGetObject_ThenFail()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			// act & assert
			memory.Cells[memory.ObjectDataType._memoryAddress] = 9;
			Assert.Throws<System.Exception>(() => memory.GetObject<DataType>(memory.ObjectDataType._memoryAddress));
		}

		[Test]
		public void GivenExactType_WhenGetObject_ThenReturnObject()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			// act
			var dataType = memory.GetObject<DataType>(memory.ObjectDataType._memoryAddress);

			// assert
			Assert.AreSame(memory.DataTypeDataType, dataType);
		}

		[Test]
		public void GivenAncestorType_WhenGetObject_ThenReturnObject()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			// act
			var dataType = memory.GetObject<Object>(memory.ObjectDataType._memoryAddress);

			// assert
			Assert.AreSame(memory.DataTypeDataType, dataType);
		}
	}
}
