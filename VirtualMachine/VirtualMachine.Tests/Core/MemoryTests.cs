using System.Collections.Generic;
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
		public void GivenNotSerialized_ThenNoDataAndReferences()
		{
			// arrange & act
			var memory = new Memory();

			var dataTypes = new HashSet<DataType>
			{
				memory.ObjectDataType,
				memory.DataTypeDataType,
				memory.DataTypeMemberDataType,
				memory.DataTypeFieldDataType,
				memory.DataTypeMethodDataType,
				memory.DataTypePropertyDataType,
				memory.DataTypeEventDataType,
				memory.DataTypeConstructorDataType,
				memory.StructureDataType,
				memory.IntegerDataType,
				memory.ArrayDataType,
				memory.CharDataType,
				memory.StringDataType,
			};

			// assert
			Assert.IsNotNull(memory.Cells);
			Assert.AreEqual(0, memory.Size);
			Assert.Greater(0, memory.NextFreeAddress);

			Assert.AreEqual(0, memory.Objects.Count);

			Assert.AreEqual(memory.Types.Count, dataTypes.Count);
			foreach (var dataType in dataTypes)
			{
				Assert.IsNotNull(dataType);
				Assert.AreEqual(dataType.Tag, dataType.Name.ToString());
				Assert.AreSame(memory.Types[dataType.Tag], dataType);
				Assert.IsFalse(dataType.IsInMemory);

				Assert.IsNull(dataType.GetDataType());
				if (dataType != memory.ObjectDataType)
				{
					Assert.IsNotNull(dataType.BaseType);
				}
				Assert.IsNotNull(dataType.Name);
				Assert.IsNotNull(dataType.Fields);
				Assert.IsNotNull(dataType.Methods);
				Assert.IsNotNull(dataType.Properties);
				Assert.IsNotNull(dataType.Events);
				Assert.IsNotNull(dataType.Constructors);
			}
		}

		[Test]
		public void GivenSerialized_ThenAllDataAndReferences()
		{
			// arrange
			var memory = new Memory();

			var objects = new List<ReferencedObject>();
			foreach (var type in memory.Types.Values)
			{
				objects.Add(type);
				objects.Add(type.Name);
				objects.Add(type.Fields);
				objects.Add(type.Methods);
				objects.Add(type.Properties);
				objects.Add(type.Events);
				objects.Add(type.Constructors);
				objects.AddRange(type.Fields);
				objects.AddRange(type.Methods);
				objects.AddRange(type.Properties);
				objects.AddRange(type.Events);
				objects.AddRange(type.Constructors);
			}

			// act
			memory.Serialize();

			// assert
			Assert.IsNotNull(memory.Cells);
			Assert.Less(0, memory.Size);
			Assert.Less(0, memory.NextFreeAddress);
			Assert.Greater(memory.NextFreeAddress, memory.Objects.Keys.Max());

			Assert.AreEqual(memory.Objects.Count, objects.Count);
			foreach (var @object in objects)
			{
				Assert.IsNotNull(@object);
				Assert.IsTrue(@object.IsInMemory);
				Assert.AreSame(@object, memory.Objects[@object.Address]);
			}

			foreach (var dataType in memory.Types.Values)
			{
				Assert.AreSame(memory.DataTypeDataType, dataType.GetDataType());
				if (dataType != memory.ObjectDataType)
				{
					Assert.IsNotNull(dataType.BaseType);
				}
				Assert.IsNotNull(dataType.Name);
				Assert.IsNotNull(dataType.Fields);
				Assert.IsNotNull(dataType.Methods);
				Assert.IsNotNull(dataType.Properties);
				Assert.IsNotNull(dataType.Events);
				Assert.IsNotNull(dataType.Constructors);
				Assert.LessOrEqual(dataType.Tag == "Structure" ? 0 : 1, dataType.GetAllFields().Count, dataType.Tag);
				Assert.LessOrEqual((MemoryAddress) dataType.Fields.Length, dataType.GetAllFields().Count);
				Assert.AreEqual(dataType.Tag, dataType.Name.ToString());
			}
		}

		[Test]
		public void GivenNoObject_WhenTryToAllocate_ThenFail()
		{
			// arrange
			var memory = new Memory();

			// act & assert
			Assert.Throws<System.ArgumentNullException>(() => memory.Allocate(null));
			memory.Serialize();
			Assert.Throws<System.ArgumentNullException>(() => memory.Allocate(null));
		}
	}
}
