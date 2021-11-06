using System;
using System.Collections.Generic;

namespace Quark
{
	// implementation of Lomuto partition scheme
	// based on pseudocode from https://en.wikipedia.org/wiki/Quicksort#Lomuto_partition_scheme
	internal static class QuickSort<T, TKey>
	{
		public static void SortInPlace(ref T[] arr, Func<T, TKey> sel, IComparer<TKey> comparer, bool reverse = false)
			=> SortInPlace(ref arr, sel, comparer, reverse, 0, arr.Length);

		public static void SortInPlace(ref T[] arr, Func<T, TKey> sel, IComparer<TKey> comparer, bool reverse, int lo, int hi)
		{
			if (lo < 0 || hi < 0 || lo >= hi) return;
			
			var pivotIndex = PartitionInPlace(ref arr, sel, comparer, reverse, lo, hi);
			// sort right side
			SortInPlace(ref arr, sel, comparer, reverse, lo, pivotIndex - 1);
			// sort left side
			// ReSharper disable once TailRecursiveCall - this code does not need the extra complexity...
			SortInPlace(ref arr, sel, comparer, reverse, pivotIndex + 1, hi);
		}

		private static int PartitionInPlace(ref T[] arr, Func<T, TKey> sel, IComparer<TKey> comparer, bool reverse, int lo, int hi)
		{
			var pivot = arr[hi];
			
			var i = lo - 1;
			for (var j = lo; j < hi; j++)
			{
				var comparison = comparer.Compare(sel(arr[j]), sel(pivot));
				// sel(arr[j]) > sel(arr[i]), or if reverse sel(arr[j]) < sel(arr[i])
				if (reverse && comparison < 0
				 || comparison > 0)
					continue;
				
				i++;
				(arr[i], arr[j]) = (arr[j], arr[i]);
			}

			return i;
		}
	}
}