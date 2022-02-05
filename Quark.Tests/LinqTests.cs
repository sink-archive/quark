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
		public void AsListGeneric() => Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list, Linq.AsList(list)));

		[Test]
		public void AsList()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list,
																	  (IEnumerable<int>)
																	  Linq.AsList(list.NonGeneric())));

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
			AssertSeqEqual(list,
						   Linq
							  .DefaultIfEmpty(list,
											  5));
		});

		[Test]
		public void DefaultIfEmptyEmpty()
			=> Array.Empty<int>()
					.AssertDoesNotMutateList(list => { AssertSeqEqual(new[] { 5 }, Linq.DefaultIfEmpty(list, 5)); });

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
			Assert.AreEqual(list[5],
							Linq.ElementAt(list, 5));
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
			new Random().GenArr(100)
						.AssertDoesNotMutateList(list2 =>
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
		public void FirstPredicate() => new[] { 5, 6, 2, 7, 8 }.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(7, Linq.First(list, a => a % 3 == 1));
		});

		[Test]
		public void FirstOrDefaultSimple() => Dataset.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(list[0],
							Linq.FirstOrDefault(list));
		});

		[Test]
		public void FirstOrDefaultSimpleEmpty()
			=> Array.Empty<int>().AssertDoesNotMutateList(list => { Assert.AreEqual(0, Linq.FirstOrDefault(list)); });

		[Test]
		public void FirstOrDefaultPredicate() => new[] { 5, 7, 2, 9 }.AssertDoesNotMutateList(list =>
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

		[Test]
		public void GroupBy() => Dataset.AssertDoesNotMutateList(list =>
		{
			var actual   = Enumerable.Cast<IGrouping<int, int>>(Linq.GroupBy(list, a => a % 3));
			var expected = Enumerable.GroupBy(list, a => a % 3);
			AssertSeqEqual(expected, actual);
		});

		[Test]
		public void Intersect() => Dataset.AssertDoesNotMutateList(list =>
		{
			new Random().GenArr(100)
						.AssertDoesNotMutateList(list2 =>
						 {
							 var actual   = Linq.Intersect(list, list2);
							 var expected = Enumerable.Intersect(list, list2);

							 AssertSeqEqual(expected, actual);
						 });
		});

		[Test]
		public void Join() => Dataset.AssertDoesNotMutateList(list =>
		{
			new Random().GenArr(100)
						.AssertDoesNotMutateList(list2 =>
						 {
							 var actual   = Linq.Join(list, list2, a => a       * 2, b => b * 3, (a, b) => a + b / 2);
							 var expected = Enumerable.Join(list, list2, a => a * 2, b => b * 3, (a, b) => a + b / 2);
							 AssertSeqEqual(expected, actual);
						 });
		});

		[Test]
		public void LastSimple() => Dataset.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(list[^1], Linq.Last(list));
		});

		[Test]
		public void LastPredicate() => new[] { 5, 6, 2, 7, 8 }.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(7, Linq.Last(list, a => a % 3 == 1));
		});

		[Test]
		public void LastOrDefaultSimple() => Dataset.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(list[^1],
							Linq.LastOrDefault(list));
		});

		[Test]
		public void LastOrDefaultSimpleEmpty()
			=> Array.Empty<int>().AssertDoesNotMutateList(list => { Assert.AreEqual(0, Linq.LastOrDefault(list)); });

		[Test]
		public void LastOrDefaultPredicate() => new[] { 5, 7, 2, 9 }.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(2, Linq.LastOrDefault(list, a => a % 2 == 0));
		});

		[Test]
		public void LastOrDefaultPredicateEmpty()
			=> Array.Empty<int>()
					.AssertDoesNotMutateList(list =>
					 {
						 Assert.AreEqual(0, Linq.LastOrDefault(list, a => a % 2 == 0));
					 });

		// NonGeneric is tested further up in CastUntyped

		[Test]
		public void OfType() => new object[] { 5, "h", false, 'c', 12, "a", true, 'b' }.AssertDoesNotMutateList(list =>
		{
			AssertSeqEqual(list.NonGeneric().OfType<int>(),    new[] { 5, 12 });
			AssertSeqEqual(list.NonGeneric().OfType<string>(), new[] { "h", "a" });
			AssertSeqEqual(list.NonGeneric().OfType<bool>(),   new[] { false, true });
			AssertSeqEqual(list.NonGeneric().OfType<char>(),   new[] { 'c', 'b' });
		});

		[Test]
		public void OrderBySimple()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.OrderBy(),
																	  Enumerable.OrderBy(list, e => e)));

		[Test]
		public void OrderByPredicate()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.OrderBy(e => 15             - e),
																	  Enumerable.OrderBy(list, e => 15 - e)));

		[Test]
		public void OrderByDescendingSimple()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.OrderByDescending(),
																	  Enumerable.OrderByDescending(list, e => e)));

		[Test]
		public void OrderByDescendingPredicate()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.OrderByDescending(e => 15             - e),
																	  Enumerable.OrderByDescending(list, e => 15 - e)));

		[Test]
		public void Prepend()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.Prepend(5), Enumerable.Prepend(list, 5)));

		[Test]
		public void Range() => AssertSeqEqual(Linq.Range(1, 500), Enumerable.Range(1, 500));

		[Test]
		public void Repeat() => AssertSeqEqual(Linq.Repeat(1, 500), Enumerable.Repeat(1, 500));

		[Test]
		public void Reverse()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.Reverse(), Enumerable.Reverse(list)));

		[Test]
		public void Select()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.Select(e => (e             + 1) / 2),
																	  Enumerable.Select(list, e => (e + 1) / 2)));

		[Test]
		public void SelectMany()
			=> Dataset.AssertDoesNotMutateList(list =>
			{
				var newSet = Enumerable.Select(list, _ => new Random().GenArr(100)).ToArray();
				AssertSeqEqual(newSet.SelectMany(), Enumerable.SelectMany(newSet, e => e));
			});

		[Test]
		public void SequenceEqual() => Dataset.AssertDoesNotMutateList(list =>
		{
			var otherList = Enumerable.ToList(Enumerable.ToArray(list));
			Assert.True(list.SequenceEqual(otherList));
			Assert.False(list.SequenceEqual(Array.Empty<object>()));
		});

		[Test]
		public void Single() => Dataset.AssertDoesNotMutateList(list =>
		{
			Assert.Throws<InvalidOperationException>(() => list.Single());
			Assert.Throws<InvalidOperationException>(() => Array.Empty<object>().Single());
			Assert.AreEqual(5, new[] { 5 }.Single());
		});

		[Test]
		public void SingleOrDefault() => Dataset.AssertDoesNotMutateList(list =>
		{
			Assert.AreEqual(default(int), list.SingleOrDefault());
			Assert.AreEqual(default,      Array.Empty<object>().SingleOrDefault());
			Assert.AreEqual(5,            new[] { 5 }.SingleOrDefault());
		});

		[Test]
		public void Skip()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.Skip(5), Enumerable.Skip(list, 5)));

		[Test]
		public void SkipLast()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.SkipLast(5), Enumerable.SkipLast(list, 5)));

		[Test]
		public void SkipWhile()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.SkipWhile(n => n             > 0),
																	  Enumerable.SkipWhile(list, n => n > 0)));
		
		[Test]
		public void Take()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.Take(5), Enumerable.Take(list, 5)));

		[Test]
		public void TakeLast()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.TakeLast(5), Enumerable.TakeLast(list, 5)));

		[Test]
		public void TakeWhile()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.TakeWhile(n => n             > 0),
																	  Enumerable.TakeWhile(list, n => n > 0)));

		[Test]
		public void ToArray()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.ToArray(), list));

		[Test]
		public void ToDictionary()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.ToDictionary(n => n             * 3),
																	  Enumerable.ToDictionary(list, n => n * 3)));

		[Test]
		public void ToHashSet()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.ToHashSet(), Enumerable.ToHashSet(list)));
		
		[Test]
		public void ToList()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.ToList(), list));

		[Test]
		public void ToLookup() => Assert.Inconclusive();
		
		[Test]
		public void Union() => Assert.Inconclusive();

		[Test]
		public void Where()
			=> Dataset.AssertDoesNotMutateList(list => AssertSeqEqual(list.Where(n => n             % 3 == 0),
																	  Enumerable.Where(list, n => n % 3 == 0)));

		[Test]
		public void Zip()
			=> Dataset.AssertDoesNotMutateList(list1 => new Random().GenArr(list1.Count)
																	.AssertDoesNotMutateList(list2
																		 => AssertSeqEqual(list1.Zip(list2),
																			 Enumerable
																				.Zip(list1, list2))));
	}
}