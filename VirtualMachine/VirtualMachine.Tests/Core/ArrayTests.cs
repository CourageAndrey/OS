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
		public void GivenOnline_WhenTryToSetItem_ThenFail()
		{
			// arrange
			var array = new Array<Integer>(new[]
			{
				new Integer { Value = 123 },
				new Integer { Value = 456 },
				new Integer { Value = 789 },
			});

			var memory = new Memory();
			array.Memory = memory;
			array.Address = 3;

			// act & assert
			Assert.Throws<System.NotImplementedException>(() => { array[1] = new Integer(); });
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