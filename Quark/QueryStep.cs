namespace Quark
{
	internal class QueryStep
	{
		public QueryStepType Type;
		public QueryStep(QueryStepType type) => Type = type;
	}

	internal enum QueryStepType
	{
		Append,
		Average,
		Cast,
		Concat,
		DefaultIfEmpty,
		Distinct,
		Except,
		GroupBy,
		GroupJoin,
		Intersect,
		Join,
		OfType,
		OrderBy,
		OrderByDescending,
		Prepend,
		Reverse,
		Select,
		SelectMany,
		Skip,
		SkipLast,
		SkipWhile,
		Take,
		TakeLast,
		TakeWhile,
		ThenBy,
		ThenByDescending,
		Union,
		Where,
		Zip
	}
}