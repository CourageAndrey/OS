using System.Linq;

using NUnit.Framework;

using VirtualMachine.Core;

namespace VirtualMachine.Tests.Core
{
	public class DataTypeTests
	{
		[Test]
		public void StaticCheck()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

			// assert
			Assert.AreSame(memory.ObjectDataType, memory.DataTypeDataType.BaseType);
			Assert.AreEqual(8, memory.DataTypeDataType.GetReferencedDataSize());
		}

		[Test]
		public void GivenAllDataTypes_WhenMemoryLoaded_ThenEverythingIsInitialized()
		{
			// arrange
			var data = Environment.LoadSample();
			var memory = new Memory(data.Item1, data.Item2);

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
			if (array.Length.Value > 0)
#warning It's necessary to make members lists unique and then remove this check.
			{
				Assert.IsFalse(allItems.Contains(array));
				allItems.Add(array);
			}
		}
	}
}