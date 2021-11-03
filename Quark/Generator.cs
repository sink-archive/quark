using System.Text;
using Microsoft.CodeAnalysis;

namespace Quark
{
	[Generator]
	public class Generator : ISourceGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{
			context.RegisterForSyntaxNotifications(() => new LinqSyntaxReceiver());
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var sb = new StringBuilder(@"
using System;
namespace Quark.LINQ
{
	public static class Queries
	{
		public static T[] Select<T>(this T[] input) => input;
		public static string ToArray<T>(this T[] input)
		// actually just lists the queries we found
		=> @""");

			if (context.SyntaxReceiver is LinqSyntaxReceiver receiver)
				foreach (var expr in receiver.Expressions)
					sb.Append(expr.GetText().ToString().Replace("\"", "\\\"") + "\n");

			sb.Append(@"
			"";
	}
}");
			
			context.AddSource("SyntaxTreeLister.cs", sb.ToString());
		}
	}
}
