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

		[Test]
		public void CastTyped() => Dataset.AssertDoesNotMutateList(list =>
		{
			var actual   = Linq.Cast<int, object>(list);
			var expected = Enumerable.Cast<object>(list);
			AssertSeqEqual(expected, actual);
		});
		
		[Test]
		public void CastUntyped() => Dataset.AssertDoesNotMutateList(list =>
		{
			var actual   = Linq.Cast<object>(list.NonGeneric());
			var expected = Enumerable.Cast<object>(list);
			AssertSeqEqual(expected, actual);
		});

		[Test]
		public void Concat() => Dataset.AssertDoesNotMutateList(list =>
		{
			new[] { 5, 6, 7 }.AssertDoesNotMutateList(list2 =>
			{
				var actual   = Linq.Concat(list, list2);
				var expected = Enumerable.Concat(list, list2);
				AssertSeqEqual(expected, actual);
			});
		});

		[Test]
		public void ContainsTrue() => new[] { 5, 7, -4, 7, 4 }.AssertDoesNotMutateList(list =>
		{
			Assert.IsTrue(Linq.Contains(list, -4));
		});
		
		[Test]
		public void ContainsFalse() => new[] { 5, 7, -4, 7, 4 }.AssertDoesNotMutateList(list =>
		{
			Assert.IsFalse(Linq.Contains(list, 9));
		});

		[Test]
		public void CountSimple() => Dataset.AssertDoesNotMutateList(list => Assert.AreEqual(100, Linq.Count(list)));
		
		[Test]
		public void CountPredicate() => Dataset.AssertDoesNotMutateList(list =>
		{
			var actual   = Linq.Count(list, a => a       % 3 == 1);
			var expected = Enumerable.Count(list, a => a % 3 == 1);
			Assert.AreEqual(expected, actual);
		});

		[Test]
		public void DefaultIfEmptyElements() => Dataset.AssertDoesNotMutateList(list =>
		{
			AssertSeqEqual(list, Linq.DefaultIfEmpty(list, 5));
		});

		[Test]
		public void DefaultIfEmptyEmpty()
			=> Array.Empty<int>()
					.AssertDoesNotMutateList(list =>
					 {
						 AssertSeqEqual(new[] { 5 }, Linq.DefaultIfEmpty(list, 5));
					 });

		[Test]
		public void Distinct() => Dataset.AssertDoesNotMutateList(list =>
		{
			var actual   = Linq.Distinct(list);
			var expected = Enumerable.Distinct(list);
			AssertSeqEqual(expected, actual);
		});

		[Test]
		public void ElementAt() => Dataset.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(list[5], Linq.ElementAt(list, 5));
		});
		
		[Test]
		public void ElementAtOrDefaultInRange() => Dataset.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(list[90], Linq.ElementAtOrDefault(list, 90));
		});

		[Test]
		public void ElementAtOrDefaultAboveRangeDefault() => Dataset.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(0, Linq.ElementAtOrDefault(list, 900));
		});

		[Test]
		public void ElementAtOrDefaultAboveRangeNull() => new[] { "hello", "world" }.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(null, Linq.ElementAtOrDefault(list, 50));
		});

		[Test]
		public void Except() => Dataset.AssertDoesNotMutateList(list =>
		{
			new Random().GenArr(100).AssertDoesNotMutateList(list2 =>
			{
				var actual   = Linq.Except(list, list2);
				var expected = Enumerable.Except(list, list2);
				
				AssertSeqEqual(expected, actual);
			});
		});

		[Test]
		public void FirstSimple() => Dataset.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(list[0], Linq.First(list));
		});
		
		[Test]
		public void FirstPredicate() => new [] {5, 4, 2, 7, 8}.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(7, Linq.First(list, a => a % 3 == 1));
		});
		
		[Test]
		public void FirstOrDefaultSimple() => Dataset.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(list[0], Linq.FirstOrDefault(list));
		});

		[Test]
		public void FirstOrDefaultSimpleEmpty()
			=> Array.Empty<int>()
					.AssertDoesNotMutateList(list =>
					 {
						 Assert.AreEqual(0, Linq.FirstOrDefault(list));
					 });
		
		[Test]
		public void FirstOrDefaultPredicate() => new [] {5, 7, 2, 9}.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(2, Linq.FirstOrDefault(list, a => a % 2 == 0));
		});

		[Test]
		public void FirstOrDefaultPredicateEmpty()
			=> Array.Empty<int>()
					.AssertDoesNotMutateList(list =>
					 {
						 Assert.AreEqual(0, Linq.FirstOrDefault(list, a => a % 2 == 0));
					 });


	}
}