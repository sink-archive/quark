using System;

namespace Quark
{
	public static class Utils
	{
		internal static QueryStepType ParseQueryStepType(string name) => name switch
		{
			"Append"            => QueryStepType.Append,
			"Average"           => QueryStepType.Average,
			"Cast"              => QueryStepType.Cast,
			"Concat"            => QueryStepType.Concat,
			"DefaultIfEmpty"    => QueryStepType.DefaultIfEmpty,
			"Distinct"          => QueryStepType.Distinct,
			"Except"            => QueryStepType.Except,
			"GroupBy"           => QueryStepType.GroupBy,
			"GroupJoin"         => QueryStepType.GroupJoin,
			"Intersect"         => QueryStepType.Intersect,
			"Join"              => QueryStepType.Join,
			"OfType"            => QueryStepType.OfType,
			"OrderBy"           => QueryStepType.OrderBy,
			"OrderByDescending" => QueryStepType.OrderByDescending,
			"Prepend"           => QueryStepType.Prepend,
			"Reverse"           => QueryStepType.Reverse,
			"Select"            => QueryStepType.Select,
			"SelectMany"        => QueryStepType.SelectMany,
			"Skip"              => QueryStepType.Skip,
			"SkipLast"          => QueryStepType.SkipLast,
			"SkipWhile"         => QueryStepType.SkipWhile,
			"Take"              => QueryStepType.Take,
			"TakeLast"          => QueryStepType.TakeLast,
			"TakeWhile"         => QueryStepType.TakeWhile,
			"ThenBy"            => QueryStepType.ThenBy,
			"ThenByDescending"  => QueryStepType.ThenByDescending,
			"Union"             => QueryStepType.Union,
			"Where"             => QueryStepType.Where,
			"Zip"               => QueryStepType.Zip,
			_                   => throw new ArgumentOutOfRangeException(nameof(name), name/*, null*/)
		};
	}
}