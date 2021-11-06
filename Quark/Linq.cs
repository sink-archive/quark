using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Quark
{
	/// <summary>
	/// Drop-in replacement for System.Linq.Enumerable optimising for performance where possible using IList
	/// </summary>
    [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
	[SuppressMessage("ReSharper", "InlineTemporaryVariable")]
	[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
	public static class Linq
	{
		public static TOut Aggregate<TIn, TOut>(this IList<TIn> source, TOut seed, Func<TOut, TIn, TOut> func)
		{
			for (var i = 0; i < source.Count; i++)
				seed = func(seed, source[i]);
			return seed;
		}

		public static bool All<T>(this IList<T> source, Predicate<T> func)
		{
			for (var i = 0; i < source.Count; i++)
				if (!func(source[i]))
					return false;

			return true;
		}
		
		public static bool Any<T>(this IList<T> source) => source.Count != 0;

		public static bool Any<T>(this IList<T> source, Predicate<T> func)
		{
			for (var i = 0; i < source.Count; i++)
				if (func(source[i]))
					return true;

			return false;
		}

		public static List<T> Append<T>(this IList<T> source, T element) => new List<T>(source) { element };

		// this seems ultra useless at first glance
		// see https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.asenumerable
		public static IEnumerable<T> AsEnumerable<T>(this IEnumerable<T> source) => source;

		// TODO: Average()

		public static List<T> Cast<T>(this IList source)
		{
			var working = new List<T>();
			foreach (T item in source) working.Add(item);
			return working;
		}

		public static List<T> Concat<T>(this IList<T> source, IList<T> second)
		{
			var tmp = new List<T>(source.Count + second.Count);
			
			for (var i = 0; i < source.Count; i++) tmp[i]                = source[i];
			for (var i = 0; i < second.Count; i++) tmp[i + source.Count] = second[i];

			return tmp;
		}

		public static bool Contains<T>(this IList<T> source, T element)
		{
			for (var i = 0; i < source.Count; i++)
				if (source[i]?.Equals(element) ?? false)
					return true;

			return false;
		}

		public static int Count<T>(this IList<T> source) => source.Count;

		public static int Count<T>(this IList<T> source, Predicate<T> predicate)
		{
			switch (source.Count)
			{
				case 0:
					return 0;
				case 1 when predicate(source[1]):
					return 1;
				default:
					var count = 0;
					for (var i = 0; i < source.Count; i++)
						if (predicate(source[i]))
							count++;

					return count;
			}
		}

		public static IList<T?> DefaultIfEmpty<T>(this IList<T> source) => source!.DefaultIfEmpty(default);
		public static IList<T> DefaultIfEmpty<T>(this IList<T> source, T defaultValue)
			=> source.Count == 0 ? new List<T> { defaultValue } : source;

		public static T[] Distinct<T>(this IEnumerable<T> source)
		{
			var tmp = Array.Empty<T>();
			new HashSet<T>(source).CopyTo(tmp);
			return tmp;
		}

		public static T ElementAt<T>(this IList<T> source, int index) => source[index];

		public static T? ElementAtOrDefault<T>(this IList<T> source, int index)
			=> index < source.Count ? source[index] : default;

		public static T? ElementAtOrDefault<T>(this T[] source, int index)
			=> index < source.Length ? source[index] : default;

		// TODO: Except()

		public static T First<T>(this IList<T> source)
			=> source.Count == 0 ? throw new InvalidOperationException($"{nameof(source)} has no elements") : source[0];

		public static T First<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source.Count == 0)
				throw new InvalidOperationException($"{nameof(source)} has no elements");

			for (var i = 0; i < source.Count; i++)
				if (predicate(source[i]))
					return source[i];

			throw new InvalidOperationException($"{nameof(source)} has no matches against {nameof(predicate)}");
		}

		public static T? FirstOrDefault<T>(this IList<T> source)
			=> source.Count == 0 ? default : source[0];
		
		public static T? FirstOrDefault<T>(this IList<T> source, Predicate<T> predicate)
		{
			for (var i = 0; i < source.Count; i++)
				if (predicate(source[i]))
					return source[i];

			return default;
		}

		// TODO: GroupBy()
		// TODO: GroupJoin()
		// TODO: Intersect()
		// TODO: Join()

		public static T Last<T>(this IList<T> source)
			=> source.Count == 0
				   ? throw new InvalidOperationException($"{nameof(source)} has no elements")
				   : source[source.Count - 1];

		public static T Last<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source.Count == 0)
				throw new InvalidOperationException($"{nameof(source)} has no elements");
			
			for (var i = source.Count - 1; i >= 0; i--)
				if (predicate(source[i]))
					return source[i];
			
			throw new InvalidOperationException($"{nameof(source)} has no matches against {nameof(predicate)}");
		}

		public static T? LastOrDefault<T>(this IList<T> source)
			=> source.Count == 0 ? default : source[source.Count - 1];
		
		public static T? LastOrDefault<T>(this IList<T> source, Predicate<T> predicate)
		{
			for (var i = source.Count - 1; i >= 0; i--)
				if (predicate(source[i]))
					return source[i];

			return default;
		}

		public static long LongCount<T>(this IList<T> source)
			=> throw new NotImplementedException($"Quark does not implement {nameof(LongCount)}");

		public static long LongCount<T>(this IList<T> source, Predicate<T> predicate)
			=> throw new NotImplementedException($"Quark does not implement {nameof(LongCount)}");

		// TODO: Max()
		// TODO: Min()

		public static List<T> OfType<T>(this IList source)
		{
			var working = new List<T>(source.Count);
			for (var i = 0; i < source.Count; i++)
				if (source[i] is T typed)
					working.Add(typed);

			return working;
		}

		public static TElem[] OrderBy<TElem, TKey>(this IList<TElem> source, Func<TElem, TKey> selector)
			=> source.OrderBy(selector, Comparer<TKey>.Default);
		
		public static TElem[] OrderBy<TElem, TKey>(this IList<TElem> source, Func<TElem, TKey> selector, IComparer<TKey> comparer)
		{
			var arr = new TElem[source.Count];
			for (var i = 0; i < source.Count; i++) 
				arr[i] = source[i];
			
			QuickSort<TElem, TKey>.SortInPlace(ref arr, selector, comparer);
			
			return arr;
		}
		
		public static TElem[] OrderByDescending<TElem, TKey>(this IList<TElem> source, Func<TElem, TKey> selector)
			=> source.OrderByDescending(selector, Comparer<TKey>.Default);

		public static TElem[] OrderByDescending<TElem, TKey>(this IList<TElem> source, Func<TElem, TKey> selector, IComparer<TKey> comparer)
		{
			var arr = new TElem[source.Count];
			for (var i = 0; i < source.Count; i++) 
				arr[i] = source[i];
			
			QuickSort<TElem, TKey>.SortInPlace(ref arr, selector, comparer, true);
			
			return arr;
		}

		public static List<T> Prepend<T>(this IList<T> source, T elem)
		{
			var tmp = new List<T>(source.Count + 1) { [0] = elem };
			for (var i = 0; i < source.Count; i++)
				tmp[i + 1] = source[i];
			
			return tmp;
		}

		public static int[] Range(int start, int count)
		{
			var working = new int[count];
			for (var i = 0; i < count; i++)
				working[i] = i + start;
			
			return working;
		}

		public static T[] Repeat<T>(T element, int count)
		{
			var working = new T[count];
			for (var i = 0; i < count; i++) 
				working[i] = element;

			return working;
		}

		public static List<T> Reverse<T>(this IList<T> source)
		{
			var tmp = new List<T>(source);
			for (var i = 0; i < tmp.Count / 2; i++)
				(tmp[i], tmp[tmp.Count - i]) = (tmp[tmp.Count - i], tmp[i]);

			return tmp;
		}

		public static List<TOut> Select<TIn, TOut>(this IList<TIn> source, Func<TIn, TOut> func)
			=> source.Select((a, _) => func(a));
		
		public static List<TOut> Select<TIn, TOut>(this IList<TIn> source, Func<TIn, int, TOut> func)
		{
			var working = new List<TOut>(source.Count);
			for (var i = 0; i < source.Count; i++) 
				working[i] = func(source[i], i);

			return working;
		}

		public static List<T> SelectMany<T>(this IList<IList<T>> source)
			// discard the index parameter here even tho its not necessary to reduce nesting of delegates
			=> source.SelectMany((a, _) => a);
		
		public static List<TOut> SelectMany<TIn, TOut>(this IList<TIn> source, Func<TIn, IList<TOut>> func)
			=> source.SelectMany((a, _) => func(a));

		public static List<TOut> SelectMany<TIn, TOut>(this IList<TIn> source, Func<TIn, int, IList<TOut>> func)
		{
			// the final size will almost certainly exceed the count, but its a good baseline
			var working = new List<TOut>(source.Count);
			for (var i = 0; i < source.Count; i++)
			{
				var sublist = func(source[i], i);
				for (var j = 0; j < sublist.Count; j++)
					working.Add(sublist[j]);
			}

			return working;
		}

		public static bool SequenceEqual<T1, T2>(this IList<T1> source, IList<T2> second)
		{
			if (source.Count != second.Count) return false;
			if (second is not IList<T1> secondTyped) return false;

			for (var i = 0; i < source.Count; i++)
			{
				if (source[i]?.Equals(secondTyped[i]) ?? false)
					return false;
			}

			return true;
		}

		public static T Single<T>(this IList<T> source)
			=> source.Count switch
			{
				0 => throw new InvalidOperationException($"{nameof(source)} has no elements"),
				1 => source[0],
				_ => throw new InvalidOperationException($"{nameof(source)} has more than one element")
			};

		public static T Single<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source.Count == 0) throw new InvalidOperationException($"{nameof(source)} has no elements");

			int? match = null;
			for (var i = 0; i < source.Count; i++)
			{
				if (!predicate(source[i])) continue;
				
				if (match.HasValue)
					throw new
						InvalidOperationException($"{nameof(source)} has multiple matches against {nameof(predicate)}");
				match = i;
			}

			if (match.HasValue)
				return source[match.Value];
			
			throw new InvalidOperationException($"{nameof(source)} has no matches against {nameof(predicate)}");
		}

		public static T? SingleOrDefault<T>(this IList<T> source) => source.Count == 1 ? source[0] : default;

		public static T? SingleOrDefault<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source.Count == 0) return default;

			int? match = null;
			for (var i = 0; i < source.Count; i++)
			{
				if (!predicate(source[i])) continue;

				if (match.HasValue) return default;
				
				match = i;
			}

			return match.HasValue ? source[match.Value] : default;
		}

		public static List<T> Skip<T>(this IList<T> source, int count)
		{
			var working = new List<T>(source.Count - count);
			for (var i = 0; i < source.Count - count; i++)
				working[i] = source[i + count];

			return working;
		}
		
		public static List<T> SkipLast<T>(this IList<T> source, int count)
		{
			var working = new List<T>(source.Count - count);
			for (var i = 0; i < source.Count - count; i++)
				working[i] = source[i];

			return working;
		}

		public static List<T> SkipWhile<T>(this IList<T> source, Predicate<T> predicate)
		{
			var working = new List<T>();
			for (var i = 0; i < source.Count; i++)
			
				if (!predicate(source[i]))
					working.Add(source[i]);

			return working;
		}
		
		// TODO: Sum()

		public static List<T> Take<T>(this IList<T> source, int count)
		{
			var working = new List<T>(count);
			for (var i = 0; i < count; i++)
				working[i] = source[i];

			return working;
		}

		public static List<T> TakeLast<T>(this IList<T> source, int count)
		{
			var working = new List<T>(count);
			for (var i = 0; i < count; i++)
				working[i] = source[i + (source.Count - count)];

			return working;
		}
		
		public static List<T> TakeWhile<T>(this IList<T> source, Predicate<T> predicate)
		{
			var working = new List<T>();
			for (var i = 0; i < source.Count; i++)
			
				if (predicate(source[i]))
					working.Add(source[i]);

			return working;
		}

		public static TElem[] ThenBy<TElem, TKey>(this IList<TElem> source, Func<TElem, TKey> selector)
			=> source.ThenBy(selector, Comparer<TKey>.Default);


		private static TElem[] ThenBy<TElem, TKey>(this IList<TElem> source, Func<TElem, TKey> selector,
												   IComparer<TKey>   comparer)
			=> source.ThenBy(selector, comparer, false);
		
		public static TElem[] ThenByDescending<TElem, TKey>(this IList<TElem> source, Func<TElem, TKey> selector)
			=> source.ThenByDescending(selector, Comparer<TKey>.Default);

		public static TElem[] ThenByDescending<TElem, TKey>(this IList<TElem> source, Func<TElem, TKey> selector,
															IComparer<TKey>   comparer)
			=> source.ThenBy(selector, comparer, true);

		private static TElem[] ThenBy<TElem, TKey>(this IList<TElem> source,   Func<TElem, TKey> selector,
												   IComparer<TKey>   comparer, bool              reverse)
		{
			var arr = new TElem[source.Count];
			for (var i = 0; i < source.Count; i++)
				arr[i] = source[i];

			var lo = 0;
			var hi = 0;
			for (var i = 1; i < source.Count; i++)
			{
				if (source[i]?.Equals(source[lo]) ?? false)
					hi = i;
				else
				{
					QuickSort<TElem, TKey>.SortInPlace(ref arr, selector, comparer, reverse, lo, hi);
					lo = i;
				}
			}

			return arr;
		}

		public static T[] ToArray<T>(this IList<T> source)
		{
			switch (source)
			{
				case T[] arr:
					return arr;
				default:
					var working = new T[source.Count];
					for (var i = 0; i < source.Count; i++) 
						working[i] = source[i];

					return working;
			}
		}

		public static Dictionary<TKey, TElem> ToDictionary<TElem, TKey>(
			this IList<TElem> source, Func<TElem, TKey> keySel)
			=> source.ToDictionary(keySel, a => a);

		public static Dictionary<TKey, TElem> ToDictionary<TIn, TKey, TElem>(
			this IList<TIn> source, Func<TIn, TKey> keySel, Func<TIn, TElem> elemSel)
		{
			var working = new Dictionary<TKey, TElem>();
			for (var i = 0; i < source.Count; i++)
				working.Add(keySel(source[i]), elemSel(source[i]));

			return working;
		}

		public static HashSet<T> ToHashSet<T>(this IList<T> source) => new(source);

		public static List<T> ToList<T>(this IEnumerable<T> source)
		{
			switch (source)
			{
				case List<T> list:
					return list;
				case IReadOnlyList<T> irol:
					var workingl = new List<T>(irol.Count);
					for (var i = 0; i < irol.Count; i++)
						workingl[i] = irol[i];

					return workingl;
				default:
				{
					var       workinge   = new List<T>();
					using var enumerator = source.GetEnumerator();
					while (enumerator.MoveNext())
						workinge.Add(enumerator.Current);
					return workinge;
				}
			}
		}

		public static Lookup<TKey, TElem> ToLookup<TIn, TKey, TElem>(this IList<TIn>  source, Func<TIn, TKey> keySel,
																	 Func<TIn, TElem> elemSel)
			=> Lookup<TKey, TElem>.Create(source, keySel, elemSel);

		//TODO: finish!
		/*public static IList<T> Union<T>(this IList<T> source, IList<T> second)
		{
			var arr = Repeat<T?>(null, source.Count + second.Count);
			var hs  = new HashSet<T>(source);
			for (var i = 0; i < second.Count; i++)
				hs.Add(second[i]);
			
			
		}*/
	}
}