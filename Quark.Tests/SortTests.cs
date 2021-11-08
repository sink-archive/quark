using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Quark.Tests
{
	public class SortTests
	{
		[SetUp]
		public void Setup() { }

		[Test]
		public void SortFullListTest()
		{
			var              dataset  = new [] { 5, 6, 3, 9, 13, 7, -5, -7, 3 };
			IEnumerable<int> expected = new [] { -7, -5, 3, 3, 5, 6, 7, 9, 13 };
			
			QuickSort<int, int>.SortInPlace(ref dataset, a => a, Comparer<int>.Default);
			
			Assert.IsTrue(dataset.SequenceEqual(expected));
		}

		[Test]
		public void SortReverseTest()
		{
			var              dataset  = new [] { 5, 6, 3, 9, 13, 7, -5, -7, 3 };
			IEnumerable<int> expected = new [] { 13, 9, 7, 6, 5, 3, 3, -5, -7 };
			
			QuickSort<int, int>.SortInPlace(ref dataset, a => a, Comparer<int>.Default, true);
			
			Assert.IsTrue(dataset.SequenceEqual(expected));
		}

		[Test]
		public void SortSectionTest()
		{
			var              dataset  = new [] { 5, 6, 3, 9, 13, 7, -5, -7, 3 };
			IEnumerable<int> expected = new [] { 5, 6, 3, -5, 7, 9, 13, -7, 3 };
			
			QuickSort<int, int>.SortInPlace(ref dataset, a => a, Comparer<int>.Default, false, 3, 6);
			
			Assert.IsTrue(dataset.SequenceEqual(expected));
		}
	}
}