using System.Text;
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
			var sb = new StringBuilder(@"
using System;
namespace Quark.Generated
{
	public static class SyntaxTreeLister
	{
		public static string List()
		{
			var working = string.Empty;");

			foreach (var syntaxTree in context.Compilation.SyntaxTrees)
			{
				sb.Append($"working += \"{syntaxTree.FilePath}\";");
			}

			sb.Append(@"
			return working;
		}
	}
}");
			context.AddSource("SyntaxTreeLister.cs", sb.ToString());
		}
	}
}