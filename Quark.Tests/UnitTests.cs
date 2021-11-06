using NUnit.Framework;
using Quark;

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

			/*// [ 6, 7, 4, 10 ]
			Assert.Pass(dataset.Select(a => a + 1).ToArray());
			// [ 10 ]
			Assert.Pass(dataset.Where(a => a >= 5).Select(a => a + 1).Where(a => a % 2 == 0).ToArray());*/
			Assert.Inconclusive();
		}
	}
}