using System.Text;
using Microsoft.CodeAnalysis;

namespace Quark
{
	[Generator]
	public class Generator : ISourceGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{
			context.RegisterForSyntaxNotifications(() => new QueryFinder());
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var sb = new StringBuilder(@"
using System;
namespace Quark.LINQ
{
	public static class Queries
	{
		public static Tout[] Select<Tin, Tout>(this Tin[] input, Func<Tin, Tout> func) => new Tout[0];
		public static string ToArray<T>(this T[] input)
		// actually just lists the queries we found
			=> @""");


			if (context.SyntaxReceiver is not QueryFinder receiver) return;
			var invocations = receiver.Invocations;
			
			foreach (var p in invocations)
				foreach (var inv in p.Value)
				{
					/*var symbol = context.Compilation.GetSemanticModel(p.Key).GetSymbolInfo(inv).Symbol;
					if (symbol == null) continue;*/
					sb.Append(inv.ToString().Replace("\"", "\"\"") + "\n");
				}

			sb.Length--;
			
			sb.Append(@""";
	}
}");
			
			context.AddSource("SyntaxTreeLister.cs", sb.ToString());
		}
	}
}