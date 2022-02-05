using BenchmarkDotNet.Attributes;

namespace Quark.Benchmarks;

public class DistinctMark : BenchmarkBase
{
	private static nint[] QuarkDistinct(IEnumerable<nint> data) => data.Distinct();

	private static nint[] LinqDistinct(IEnumerable<nint> data) => Enumerable.Distinct(data).ToArray();

	[Benchmark]
	public nint[] QuarkDistinctTiny() => QuarkDistinct(MainDataTiny);
	
	[Benchmark]
	public nint[] LinqDistinctTiny() => LinqDistinct(MainDataTiny);

	[Benchmark]
	public nint[] QuarkDistinctSmall() => QuarkDistinct(MainDataSmall);

	[Benchmark]
	public nint[] LinqDistinctSmall() => LinqDistinct(MainDataSmall);

	[Benchmark]
	public nint[] QuarkDistinctMedium() => QuarkDistinct(MainDataMedium);

	[Benchmark]
	public nint[] LinqDistinctMedium() => LinqDistinct(MainDataMedium);

	[Benchmark]
	public nint[] QuarkDistinctLarge() => QuarkDistinct(MainDataLarge);

	[Benchmark]
	public nint[] LinqDistinctLarge() => LinqDistinct(MainDataLarge);
}