using BenchmarkDotNet.Running;
using Quark.Benchmarks;

BenchmarkSwitcher.FromAssembly(typeof(SelectMark).Assembly).RunAllJoined();