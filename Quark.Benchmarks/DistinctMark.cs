using BenchmarkDotNet.Attributes;

namespace Quark.Benchmarks;

public class DistinctMark : BenchmarkBase
{
	private static nint[] QuarkToHashSet(IEnumerable<nint> data) => data.Distinct();

	private static nint[] LinqToHashSet(IEnumerable<nint> data) => Enumerable.Distinct(data).ToArray();

	[Benchmark]
	public nint[] QuarkSelectSmall() => QuarkToHashSet(MainDataSmall);

	[Benchmark]
	public nint[] LinqSelectSmall() => LinqToHashSet(MainDataSmall);

	[Benchmark]
	public nint[] QuarkSelectMedium() => QuarkToHashSet(MainDataMedium);

	[Benchmark]
	public nint[] LinqSelectMedium() => LinqToHashSet(MainDataMedium);

	[Benchmark]
	public nint[] QuarkSelectLarge() => QuarkToHashSet(MainDataLarge);

	[Benchmark]
	public nint[] LinqSelectLarge() => LinqToHashSet(MainDataLarge);
}