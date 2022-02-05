using BenchmarkDotNet.Attributes;

namespace Quark.Benchmarks;

public class SortMark : BenchmarkBase
{
	private static nint[] QuarkSort(IList<nint> data) => data.OrderBy();

	private static nint[] LinqSort(IEnumerable<nint> data) => data.OrderBy(n => n).ToArray();

	private static nint[] SortInPlace(nint[] data)
	{
		QuickSort<nint, nint>.SortInPlace(ref data, e => e, Comparer<nint>.Default);
		return data;
	}

	[IterationCleanup(Targets = new[]
							 { nameof(SortInPlaceSmall), nameof(SortInPlaceMedium), nameof(SortInPlaceLarge) })]
	public void IterationCleanup() => ReinitDatasets();

	[Benchmark]
	public nint[] QuarkSortTiny() => QuarkSort(MainDataTiny);

	[Benchmark]
	public nint[] LinqSortTiny() => LinqSort(MainDataTiny);

	[Benchmark]
	public nint[] SortInPlaceTiny() => SortInPlace(MainDataTiny);
	
	[Benchmark]
	public nint[] QuarkSortSmall() => QuarkSort(MainDataSmall);

	[Benchmark]
	public nint[] LinqSortSmall() => LinqSort(MainDataSmall);

	[Benchmark]
	public nint[] SortInPlaceSmall() => SortInPlace(MainDataSmall);

	[Benchmark]
	public nint[] QuarkSortMedium() => QuarkSort(MainDataMedium);

	[Benchmark]
	public nint[] LinqSortMedium() => LinqSort(MainDataMedium);

	[Benchmark]
	public nint[] SortInPlaceMedium() => SortInPlace(MainDataMedium);

	[Benchmark]
	public nint[] QuarkSortLarge() => QuarkSort(MainDataLarge);


	[Benchmark]
	public nint[] LinqSortLarge() => LinqSort(MainDataLarge);


	[Benchmark]
	public nint[] SortInPlaceLarge() => SortInPlace(MainDataLarge);
}