using NUnit.Framework;

namespace Quark.Tests
{
	public class UnitTests
	{
		[SetUp]
		public void Setup() { }

		[Test]
		public void DemoTest()
		{
			Assert.AreEqual("Hello, World!", Quark.Generated.Test.Main());
		}
	}
}