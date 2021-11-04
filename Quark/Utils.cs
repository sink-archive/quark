using System;
using System.Linq;
using System.Text;

namespace Quark
{
	public static class Utils
	{
		internal static QueryStepType ParseQueryStepType(string name) => name switch
		{
			"Append" => QueryStepType.Append,
			"Average" => QueryStepType.Average,
			"Cast" => QueryStepType.Cast,
			"Concat" => QueryStepType.Concat,
			"DefaultIfEmpty" => QueryStepType.DefaultIfEmpty,
			"Distinct" => QueryStepType.Distinct,
			"Except" => QueryStepType.Except,
			"GroupBy" => QueryStepType.GroupBy,
			"GroupJoin" => QueryStepType.GroupJoin,
			"Intersect" => QueryStepType.Intersect,
			"Join" => QueryStepType.Join,
			"OfType" => QueryStepType.OfType,
			"OrderBy" => QueryStepType.OrderBy,
			"OrderByDescending" => QueryStepType.OrderByDescending,
			"Prepend" => QueryStepType.Prepend,
			"Reverse" => QueryStepType.Reverse,
			"Select" => QueryStepType.Select,
			"SelectMany" => QueryStepType.SelectMany,
			"Skip" => QueryStepType.Skip,
			"SkipLast" => QueryStepType.SkipLast,
			"SkipWhile" => QueryStepType.SkipWhile,
			"Take" => QueryStepType.Take,
			"TakeLast" => QueryStepType.TakeLast,
			"TakeWhile" => QueryStepType.TakeWhile,
			"ThenBy" => QueryStepType.ThenBy,
			"ThenByDescending" => QueryStepType.ThenByDescending,
			"Union" => QueryStepType.Union,
			"Where" => QueryStepType.Where,
			"Zip" => QueryStepType.Zip,
			_ => throw new ArgumentOutOfRangeException(nameof(name), $"{name} is not a valid query step type")
		};

		internal static QueryEndType ParseQueryEndType(string name) => name switch
		{
			"Aggregate" => QueryEndType.Aggregate,
			"All" => QueryEndType.All,
			"Any" => QueryEndType.Any,
			"Average" => QueryEndType.Average,
			"Contains" => QueryEndType.Contains,
			"Count" => QueryEndType.Count,
			"ElementAt" => QueryEndType.ElementAt,
			"ElementAtOrDefault" => QueryEndType.ElementAtOrDefault,
			"First" => QueryEndType.First,
			"FirstOrDefault" => QueryEndType.FirstOrDefault,
			"Last" => QueryEndType.Last,
			"LastOrDefault" => QueryEndType.LastOrDefault,
			"LongCount" => QueryEndType.LongCount,
			"Max" => QueryEndType.Max,
			"Min" => QueryEndType.Min,
			"SequenceEqual" => QueryEndType.SequenceEqual,
			"Single" => QueryEndType.Single,
			"SingleOrDefault" => QueryEndType.SingleOrDefault,
			"Sum" => QueryEndType.Sum,
			"ToArray" => QueryEndType.ToArray,
			"ToDictionary" => QueryEndType.ToDictionary,
			"ToHashSet" => QueryEndType.ToHashSet,
			"ToList" => QueryEndType.ToList,
			"ToLookup" => QueryEndType.ToLookup,
			"ToEnumerable" => QueryEndType.ToEnumerable,
			_ => throw new ArgumentOutOfRangeException(nameof(name), $"{name} is not a valid query end type")
		};

		// ReSharper disable once InconsistentNaming
		public static string GetILStr(this Delegate @delegate)
			=> Encoding.UTF8.GetString(@delegate.Method.GetMethodBody()?.GetILAsByteArray() ?? Array.Empty<byte>());
	}
}