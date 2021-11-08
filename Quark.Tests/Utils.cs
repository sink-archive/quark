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
				Assert.IsTrue(first.SequenceEqual(secondTyped));
			else
				Assert.Fail($"{nameof(second)} was type {typeof(T2).FullName} instead of {typeof(T1).FullName}");
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