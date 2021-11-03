using NUnit.Framework;
using Quark.LINQ;

namespace Quark.Tests
{
	public class UnitTests
	{
		[SetUp]
		public void Setup() { }

		[Test]
		public void BasicTest()
		{
			var dataset = new[] { 5, 6, 3, 9 };

			Assert.Pass(dataset.Select<int, int>(a => a + 1).ToArray());
		}
	}
}