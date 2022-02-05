using System.Diagnostics.CodeAnalysis;

namespace Quark.Benchmarks;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class BenchmarkBase
{
	private const int TinySize   = 100;
	private const int SmallSize  = 100_000;
	private const int MediumSize = 1_000_000;
	private const int LargeSize  = 10_000_000;

	protected readonly nint[] MainDataTiny       = new nint[TinySize];
	protected readonly nint[] SecondaryDataTiny  = new nint[TinySize];
	protected readonly nint[] MainDataSmall       = new nint[SmallSize];
	protected readonly nint[] SecondaryDataSmall  = new nint[SmallSize];
	protected readonly nint[] MainDataMedium      = new nint[MediumSize];
	protected readonly nint[] SecondaryDataMedium = new nint[MediumSize];
	protected readonly nint[] MainDataLarge       = new nint[LargeSize];
	protected readonly nint[] SecondaryDataLarge  = new nint[LargeSize];

	protected BenchmarkBase() => ReinitDatasets();

	protected void ReinitDatasets()
	{
		var rand = new Random();

		nint RandomNativeInt() => (nint) rand.NextInt64(nint.MinValue, nint.MinValue);

		for (var i = 0; i < TinySize; i++)
		{
			MainDataTiny[i]      = RandomNativeInt();
			SecondaryDataTiny[i] = RandomNativeInt();
		}
		
		for (var i = 0; i < SmallSize; i++)
		{
			MainDataSmall[i]      = RandomNativeInt();
			SecondaryDataSmall[i] = RandomNativeInt();
		}

		for (var i = 0; i < MediumSize; i++)
		{
			MainDataMedium[i]      = RandomNativeInt();
			SecondaryDataMedium[i] = RandomNativeInt();
		}

		for (var i = 0; i < LargeSize; i++)
		{
			MainDataLarge[i]      = RandomNativeInt();
			SecondaryDataLarge[i] = RandomNativeInt();
		}
	}
}