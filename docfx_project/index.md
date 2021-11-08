# Welcome to the Quark documentation

Quark is a high performance drop-in replacement for the .NET LINQ library.

Click [here](api/Quark) to see API Reference documentation.

Quark does make some changes compared to LINQ, however. To see these changes, check the [*Changes article*](articles/changes.md).

Quark is designed to be [as fast as possible](articles/optimisations.md), which leads to the key change of removing lazy query evaluation.
This allows huge speedups, especially in sorting performance.

Benchmarks are on the [benchmarks page](articles/benchmarks.md), but I'll attach some pretty graphs here anyway once I get chance.