using System;
using System.Collections;
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
			if (second is not IEnumerable<T1> secondTyped)
				Assert.Fail($"{nameof(second)} was type {typeof(T2).FullName} instead of {typeof(T1).FullName}");
			else if (!SeqEqualRec(first, secondTyped))
				Assert.Fail("Sequences were not recursively equal");
		}

		private static bool SeqEqualRec(IEnumerable first, IEnumerable second)
		{
			// both must be of the same type
			/*if (second is not IEnumerable<T1> secondT)
				return false;*/
				
			var (e1, e2) = (first.GetEnumerator(), /*secondT*/second.GetEnumerator());
			while (true)
			{
				var (m1, m2) = (e1.MoveNext(), e2.MoveNext());

				// if any list has finished
				if (!m1 || !m2)
					// both must be finished
					return !m1 && !m2;
				
				var (n1, n2) = (e1.Current, e2.Current);
				// if both are enumerables then recursively test
				if (n1 is IEnumerable n1E && n2 is IEnumerable n2E)
				{
					if (!SeqEqualRec(n1E, n2E))
						return false;
				}
				// else just test if they are equal
				else if (!n1.Equals(n2) && !n2.Equals(n1))
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