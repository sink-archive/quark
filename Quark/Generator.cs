using Microsoft.CodeAnalysis;

namespace Quark
{
	[Generator]
	public class Generator : ISourceGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{
			// we don't need to initialise anything for this generator :)
		}

		public void Execute(GeneratorExecutionContext context)
		{
			context.AddSource("helloworld.cs", @"
using System;
namespace Quark.Generated {
	public static class Test {
		public static string Main() => ""Hello, World!"";
	} 
}");
		}
	}
}