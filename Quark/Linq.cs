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

		public static IList<T> Append<T>(this IList<T> source, T element) => new List<T>(source) { element };

		// this seems ultra useless at first glance
		// see https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.asenumerable
		public static IEnumerable<T> AsEnumerable<T>(this IEnumerable<T> source) => source;

		// TODO: Average()

		public static IList<T> Cast<T>(this IList source)
		{
			var working = new List<T>();
			foreach (T item in source) working.Add(item);
			return working;
		}

		public static IList<T> Concat<T>(this IList<T> source, IList<T> second)
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

		public static IList<T> Distinct<T>(this IEnumerable<T> source)
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

		public static IList<T> OfType<T>(this IList source)
		{
			var working = new List<T>(source.Count);
			for (var i = 0; i < source.Count; i++)
				if (source[i] is T typed)
					working.Add(typed);

			return working;
		}
	}
}