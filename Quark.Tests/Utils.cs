using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace Quark.Tests
{
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
	public static class Utils
	{
		public static int RandInt(this Random rand)
			=> rand.Next(int.MinValue, int.MaxValue);

		public static int[] GenArr(this Random rand, int count)
			=> Enumerable.Range(rand.RandInt(), 100).Select(_ => rand.RandInt()).ToArray();

		public static void AssertSeqEqual<T1, T2>(IEnumerable<T1> first, IEnumerable<T2> second)
		{
			if (second is IEnumerable<T1> secondTyped)
				Assert.IsTrue(SeqEqualRec(first, secondTyped));
			else
				Assert.Fail($"{nameof(second)} was type {typeof(T2).FullName} instead of {typeof(T1).FullName}");
		}

		private static bool SeqEqualRec<T1, T2>(IEnumerable<T1> first, IEnumerable<T2> second)
		{
			if (second is not IEnumerable<T1> secondT)
				return false;
				
			var (e1, e2) = (first.GetEnumerator(), secondT.GetEnumerator());
			while (true)
			{
				var (m1, m2) = (e1.MoveNext(), e2.MoveNext());

				if (!(m1 && m2))
					return (!m1 && !m2);
				
				var (n1, n2) = (e1.Current, e2.Current);
				if (n1 is IEnumerable<T1> n1e && n2 is IEnumerable<T1> n2e)
					if (!SeqEqualRec(n1e, n2e))
						return false;
				else
					if (!Equals(n1, n2))
						return false;
			}
		}

		public static T[] CopySequence<T>(this IEnumerable<T> sequence) => sequence.ToArray();

		public static void AssertDoesNotMutateList<T>(this IList<T> seq, Action<IList<T>> action)
		{
			var original = seq.CopySequence();
			action(seq);
			AssertSeqEqual(seq, original);
		}
	}
}