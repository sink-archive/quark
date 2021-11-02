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
			Assert.Pass(Generated.SyntaxTreeLister.List());
		}
	}
}