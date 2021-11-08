using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Quark.Tests.Utils;

namespace Quark.Tests
{
	[TestFixture]
	public class LinqTests
	{
		public static readonly int[] Dataset = new Random().GenArr(100);

		[Test]
		public void AggregateTest() => Dataset.AssertDoesNotMutateList(list =>
		{
			string Func(string current, int next) => current + (current.Length + 1) * next;

			var actual   = list.Aggregate("", Func);
			var expected = ((IEnumerable<int>) list).Aggregate("", Func);

			Assert.AreEqual(expected, actual);
		});
	}
}