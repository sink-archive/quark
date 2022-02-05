using BenchmarkDotNet.Attributes;

namespace Quark.Benchmarks;

public class WhereMark : BenchmarkBase
{
	private static List<nint> QuarkWhere(IList<nint> data) => data.Where((n, i) => n % i == 0);

	private static nint[] LinqWhere(IEnumerable<nint> data) => data.Where((n, i) => n % i == 0).ToArray();

	[Benchmark]
	public List<nint> QuarkWhereTiny() => QuarkWhere(MainDataTiny);
	
	[Benchmark]
	public nint[] LinqWhereTiny() => LinqWhere(MainDataTiny);

	[Benchmark]
	public List<nint> QuarkWhereSmall() => QuarkWhere(MainDataSmall);

	[Benchmark]
	public nint[] LinqWhereSmall() => LinqWhere(MainDataSmall);

	[Benchmark]
	public List<nint> QuarkWhereMedium() => QuarkWhere(MainDataMedium);

	[Benchmark]
	public nint[] LinqWhereMedium() => LinqWhere(MainDataMedium);

	[Benchmark]
	public List<nint> QuarkWhereLarge() => QuarkWhere(MainDataLarge);

	[Benchmark]
	public nint[] LinqWhereLarge() => LinqWhere(MainDataLarge);
}