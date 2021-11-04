using System.Collections.Generic;

namespace Quark
{
	internal class QueryEnd
	{
		public QueryEndType          Type;
		public LinkedList<QueryStep> ReversedSteps;
		public QueryEnd(QueryEndType type, LinkedList<QueryStep> reversedSteps)
		{
			Type          = type;
			ReversedSteps = reversedSteps;
		}
	}
	
	internal enum QueryEndType
	{
		Aggregate,
		All,
		Any,
		Average,
		Contains,
		Count,
		ElementAt,
		ElementAtOrDefault,
		First,
		FirstOrDefault,
		Last,
		LastOrDefault,
		LongCount,
		Max,
		Min,
		SequenceEqual,
		Single,
		SingleOrDefault,
		Sum,
		ToArray,
		ToDictionary,
		ToHashSet,
		ToList,
		ToLookup,
		ToEnumerable
	}
}