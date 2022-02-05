using BenchmarkDotNet.Attributes;

namespace Quark.Benchmarks;

public class SelectMark : BenchmarkBase
{
	private static nint[] QuarkSelect(IList<nint> data) => data.Select(n => 2 * n / 3);

	private static nint[] LinqSelect(IEnumerable<nint> data) => data.Select(n => 2 * n / 3).ToArray();

	[Benchmark]
	public nint[] QuarkSelectTiny() => QuarkSelect(MainDataTiny);
	
	[Benchmark]
	public nint[] LinqSelectTiny() => LinqSelect(MainDataTiny);
	
	[Benchmark]
	public nint[] QuarkSelectSmall() => QuarkSelect(MainDataSmall);

	[Benchmark]
	public nint[] LinqSelectSmall() => LinqSelect(MainDataSmall);

	[Benchmark]
	public nint[] QuarkSelectMedium() => QuarkSelect(MainDataMedium);

	[Benchmark]
	public nint[] LinqSelectMedium() => LinqSelect(MainDataMedium);

	[Benchmark]
	public nint[] QuarkSelectLarge() => QuarkSelect(MainDataLarge);

	[Benchmark]
	public nint[] LinqSelectLarge() => LinqSelect(MainDataLarge);
}