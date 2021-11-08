# Welcome to the Quark documentation

Quark is a high performance drop-in replacement for the .NET LINQ library.

Click [here](xref:Quark) to see API Reference documentation.

Quark does make some changes compared to LINQ, however. To see these changes, check the [*Changes article*](articles/changes.md).

Quark is designed to be [as fast as possible](articles/optimisations.md), which leads to the key change of removing lazy query evaluation.
This allows huge speedups, especially in sorting performance.

Also of note is that LINQ has some weird quirks that may confuse (nothing Quark-specific - there isnt anything specific - just LINQ in general).
Make sure to check out our [clarifications](articles/clarifications.md)!

Benchmarks are on the [benchmarks page](articles/benchmarks.md), but I'll attach some pretty graphs here anyway once I get chance.