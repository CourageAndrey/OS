using NUnit.Framework;

using VirtualMachine.Core;
using VirtualMachine.Reflection;

namespace VirtualMachine.Tests.Core
{
	public class DataTypeTests
	{
		[Test]
		public void GivenAllDataTypes_WhenMemoryLoaded_ThenEverythingIsInitialized()
		{
			// arrange
			var memory = new Memory();

			var references = new System.Collections.Generic.HashSet<object>();

			// assert
			foreach (var dataType in memory.Types.Values)
			{
				CheckForNotNullAndUnique(dataType.Constructors, references);
				CheckForNotNullAndUnique(dataType.Events, references);
				CheckForNotNullAndUnique(dataType.Fields, references);
				CheckForNotNullAndUnique(dataType.Methods, references);
				CheckForNotNullAndUnique(dataType.Properties, references);
			}
		}

		private void CheckForNotNullAndUnique(Array array, System.Collections.Generic.ICollection<object> allItems)
		{
			Assert.IsNotNull(array);
			Assert.IsFalse(allItems.Contains(array));
			allItems.Add(array);
		}

		[Test]
		public void GivenNoName_WhenTryToCreateDataType_ThenFail()
		{
			// arrange
			var memory = new Memory();

			// act & assert
			Assert.Throws<System.ArgumentNullException>(() => new DataType(memory, null, null, new DataTypeField[0]));
			Assert.Throws<System.ArgumentNullException>(() => new DataType(memory, string.Empty, null, new DataTypeField[0]));
		}

		[Test]
		public void GivenNoMemory_WhenTryToCreateDataType_ThenFail()
		{
			// act & assert
			Assert.Throws<System.NullReferenceException>(() => new DataType(null, "test", null, new DataTypeField[0]));
		}

		[Test]
		public void GivenDifferentTypes_WhenGetDataSize_ThenReturnNumberOfFields()
		{
			// arrange
			var memory = new Memory();

			// act & assert
			Assert.AreEqual(1, memory.ObjectDataType.GetDataSize());
			Assert.AreEqual(8, memory.DataTypeDataType.GetDataSize());
			Assert.AreEqual(2, memory.DataTypeMemberDataType.GetDataSize());
			Assert.AreEqual(2, memory.DataTypeFieldDataType.GetDataSize());
			Assert.AreEqual(2, memory.DataTypeMethodDataType.GetDataSize());
			Assert.AreEqual(2, memory.DataTypePropertyDataType.GetDataSize());
			Assert.AreEqual(2, memory.DataTypeEventDataType.GetDataSize());
			Assert.AreEqual(2, memory.DataTypeConstructorDataType.GetDataSize());
			Assert.AreEqual(0, memory.StructureDataType.GetDataSize());
			Assert.AreEqual(1, memory.IntegerDataType.GetDataSize());
			Assert.AreEqual(2, memory.ArrayDataType.GetDataSize());
			Assert.AreEqual(1, memory.CharDataType.GetDataSize());
			Assert.AreEqual(2, memory.StringDataType.GetDataSize());
		}
	}
}