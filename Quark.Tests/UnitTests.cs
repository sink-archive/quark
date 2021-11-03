using NUnit.Framework;
using Quark.Linq;

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

			Assert.Pass(dataset.Select(a => a + 1).ToArray());
		}
	}
}