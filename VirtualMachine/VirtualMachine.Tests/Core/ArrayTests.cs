using System.Linq;

using NUnit.Framework;

using VirtualMachine.Core;

namespace VirtualMachine.Tests.Core
{
	public class ArrayTests
	{
		[Test]
		public void GivenUntypedVersion_WhenGetEnumerator_ThenWorksTheSame()
		{
			// arrange
			var array = new Array<Integer>(new[]
			{
				new Integer { Value = 123 },
				new Integer { Value = 456 },
				new Integer { Value = 789 },
			});

			// act
			var untyped = array as System.Collections.IEnumerable;

			var list = new System.Collections.Generic.List<ulong>();
			foreach (var @int in untyped)
			{
				list.Add(((Integer) @int).Value);
			}

			// assert
			Assert.IsTrue(array.Select(i => i.Value).SequenceEqual(list));
		}

		[Test]
		public void GivenOffline_WhenSetItem_ThenSucceed()
		{
			// arrange
			var array = new Array<Integer>(new[]
			{
				new Integer { Value = 123 },
				new Integer { Value = 456 },
				new Integer { Value = 789 },
			});

			// act
			array[1] = new Integer { Value = 999 };

			// assert
			Assert.AreEqual(999, array[1].Value);
		}

		[Test]
		public void GivenOnline_WhenSetItem_ThenSucceed()
		{
			// arrange
			var array = new Array<Integer>(new[]
			{
				new Integer { Value = 123 },
				new Integer { Value = 456 },
				new Integer { Value = 789 },
			});

			var memory = new Memory();
			memory.Serialize();

			memory.Store(array);

			// pre-assert
			Assert.AreSame(memory.ArrayDataType, array.GetDataType());
			Assert.AreEqual(3, array.Length);
			Assert.AreEqual(123, array[0].Value);
			Assert.AreEqual(456, array[1].Value);
			Assert.AreEqual(789, array[2].Value);

			// act
			array[1] = new Integer { Value = 654 };

			// assert
			Assert.AreEqual(654, array[1].Value);
		}

		[Test]
		public void GivenUntypedArray_WhenGetEnumeratorAndLength_ThenWorksAsUsual()
		{
			// arrange
			Array array = new Array<Integer>(new[]
			{
				new Integer { Value = 123 },
				new Integer { Value = 456 },
				new Integer { Value = 789 },
			});

			// act
			var list = new System.Collections.Generic.List<ulong>();
			foreach (var @int in array)
			{
				list.Add(((Integer) @int).Value);
			}

			// assert
			Assert.AreEqual(3, array.Length);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(123, list[0]);
			Assert.AreEqual(456, list[1]);
			Assert.AreEqual(789, list[2]);
		}
	}
}