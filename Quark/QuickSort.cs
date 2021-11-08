using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Quark.Tests")]

namespace Quark
{
	// implementation of Hoare partition scheme
	// based on pseudocode from https://en.wikipedia.org/wiki/Quicksort#Hoare_partition_scheme
	internal static class QuickSort<T, TKey>
	{
		public static void SortInPlace(ref T[] arr, Func<T, TKey> sel, IComparer<TKey> comparer, bool reverse = false)
			=> SortInPlace(ref arr, sel, comparer, reverse, 0, arr.Length - 1);

		public static void SortInPlace(ref T[] arr, Func<T, TKey> sel, IComparer<TKey> comparer, bool reverse, int lo, int hi)
		{
			if (lo < 0 || hi < 0 || lo >= hi) return;
			
			var pivotIndex = PartitionInPlace(ref arr, sel, comparer, reverse, lo, hi);
			SortInPlace(ref arr, sel, comparer, reverse, lo, pivotIndex);
			// ReSharper disable once TailRecursiveCall - this code does not need the extra complexity...
			SortInPlace(ref arr, sel, comparer, reverse, pivotIndex + 1, hi);
		}

		private static int PartitionInPlace(ref T[] arr, Func<T, TKey> sel, IComparer<TKey> comparer, bool reverse, int lo, int hi)
		{
			// middle of range
			var pivot = sel(arr[lo + (hi - lo) / 2]);

			var i = lo - 1;
			var j = hi + 1;

			while (true)
			{
				do i++;
				while (reverse ? GT(sel(arr[i]), pivot, comparer) : LT(sel(arr[i]), pivot, comparer));
				
				do j--;
				while (reverse ? LT(sel(arr[j]), pivot, comparer) : GT(sel(arr[j]), pivot, comparer));

				if (i >= j) return j;

				(arr[i], arr[j]) = (arr[j], arr[i]);
			}
		}

		// ReSharper disable InconsistentNaming
		private static bool LT<TC>(TC a, TC b, IComparer<TC> comp)
			=> comp.Compare(a, b) < 0;
		
		private static bool GT<TC>(TC a, TC b, IComparer<TC> comp)
			=> comp.Compare(a, b) > 0;
		// ReSharper restore InconsistentNaming
	}
}