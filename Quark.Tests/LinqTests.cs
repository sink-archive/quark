using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using static Quark.Tests.Utils;

namespace Quark.Tests
{
	[TestFixture]
	[SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
	public class LinqTests
	{
		public static readonly int[] Dataset = new Random().GenArr(100);

		[Test]
		public void Aggregate() => Dataset.AssertDoesNotMutateList(list =>
		{
			string Func(string current, int next) => current + (current.Length + 1) * next;

			var actual   = list.Aggregate("", Func);
			var expected = ((IEnumerable<int>) list).Aggregate("", Func);

			Assert.AreEqual(expected, actual);
		});

		[Test]
		public void AllTrue()
			=> ((IEnumerable<int>) Dataset).Select(a => a - a % 2)
										   .ToArray()
										   .AssertDoesNotMutateList(list =>
											{
												Assert.IsTrue(list.All(a => a % 2 == 0));
											});

		[Test]
		public void AllFalse()
			=> ((IEnumerable<int>) Dataset).Select((a, i) => i == 5 ? a - 1 - a % 2 : a - a % 2)
										   .ToArray()
										   .AssertDoesNotMutateList(list =>
											{
												Assert.IsFalse(list.All(a => a % 2 == 0));
											});
											
		[Test]
		public void AnyTrue()
			=> ((IEnumerable<int>) Dataset).Select((a, i) => i == 5 ? a / 2 : a)
										   .ToArray()
										   .AssertDoesNotMutateList(list =>
											{
												Assert.IsTrue(list.Any(a => a % 2 == 0));
											});
		
		[Test]
		public void AnyFalse()
			=> ((IEnumerable<int>) Dataset).Select(a => a - 1 - a % 2)
										   .ToArray()
										   .AssertDoesNotMutateList(list =>
											{
												Assert.IsFalse(list.All(a => a % 2 == 0));
											});

		[Test]
		public void AnyItems() 
			=> Dataset.AssertDoesNotMutateList(list => Assert.IsTrue(list.Any()));

		[Test]
		public void AnyEmpty()
			=> Array.Empty<int>().AssertDoesNotMutateList(list => Assert.IsFalse(list.Any()));

		[Test]
		public void Append() => Dataset.AssertDoesNotMutateList(list =>
		{
			var actual   = Linq.Append(list, 5); // i like 5 :)
			var expected = Enumerable.Append(list, 5);
			AssertSeqEqual(expected, actual);
		});

		[Test]
		public void AsEnumerable() => Dataset.AssertDoesNotMutateList(list =>
		{
			var actual   = Linq.AsEnumerable(list);
			var expected = Enumerable.AsEnumerable(list);
			AssertSeqEqual(expected, actual);
		});
	}
}