using System.Linq;
using NUnit.Framework;

namespace Quark.Tests
{
	public class UnitTests
	{
		[SetUp]
		public void Setup() { }

		[Test]
		public void BasicTest()
		{
			var dataset = new [] { 5, 6, 3, 9 };

			var newset = dataset.Append(7);
			
			Assert.True(dataset.SequenceEqual(new [] {5, 6, 3, 9}));
		}
	}
}