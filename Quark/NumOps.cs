using System;
using System.Collections.Generic;

namespace Quark
{
	public static partial class Linq
	{
		public static decimal Average(this IList<decimal> source) => source.Sum() / source.Count;
		public static double  Average(this IList<double>  source) => source.Sum() / source.Count;
		public static float   Average(this IList<float>   source) => source.Sum() / source.Count;
		public static long    Average(this IList<long>    source) => source.Sum() / source.Count;
		public static int     Average(this IList<int>     source) => source.Sum() / source.Count;
		
		public static decimal Max(this IList<decimal> source) => source.Aggregate(decimal.MaxValue, Math.Max);
		public static double  Max(this IList<double>  source) => source.Aggregate(double.MaxValue,  Math.Max);
		public static float   Max(this IList<float>   source) => source.Aggregate(float.MaxValue,   Math.Max);
		public static long    Max(this IList<long>    source) => source.Aggregate(long.MaxValue,    Math.Max);
		public static int     Max(this IList<int>     source) => source.Aggregate(int.MaxValue,     Math.Max);
		
		public static decimal Min(this IList<decimal> source) => source.Aggregate(decimal.MaxValue, Math.Min);
		public static double  Min(this IList<double>  source) => source.Aggregate(double.MaxValue,  Math.Min);
		public static float   Min(this IList<float>   source) => source.Aggregate(float.MaxValue,   Math.Min);
		public static long    Min(this IList<long>    source) => source.Aggregate(long.MaxValue,    Math.Min);
		public static int     Min(this IList<int>     source) => source.Aggregate(int.MaxValue,     Math.Min);
		
		public static decimal Sum(this IList<decimal> source) => source.Aggregate(decimal.MaxValue, (c, n) => c + n);
		public static double  Sum(this IList<double>  source) => source.Aggregate(double.MaxValue,  (c, n) => c + n);
		public static float   Sum(this IList<float>   source) => source.Aggregate(float.MaxValue,   (c, n) => c + n);
		public static long    Sum(this IList<long>    source) => source.Aggregate(long.MaxValue,    (c, n) => c + n);
		public static int     Sum(this IList<int>     source) => source.Aggregate(int.MaxValue,     (c, n) => c + n);
	}
}