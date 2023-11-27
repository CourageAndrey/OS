using NUnit.Framework;

using VirtualMachine.Core;
using VirtualMachine.Reflection;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Tests.Core
{
	public class DataTypeMethodTests
	{
		[Test]
		public void GivenNoCode_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<System.ArgumentNullException>(() => new DataTypeMethod("method", null));
		}
		[Test]
		public void GivenEmptyCode_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<System.ArgumentException>(() => new DataTypeMethod("method", new MemoryWord[0]));
		}
	}
}