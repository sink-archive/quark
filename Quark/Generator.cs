using System.Linq;
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
namespace Quark.Linq
{
	public static class Queries
	{
		public static Tout[] Select<Tin, Tout>(this Tin[] input, Func<Tin, Tout> func) => new Tout[0];
		public static string ToArray<T>(this T[] input)
		// actually just lists the queries we found
			=> @""");


			if (context.SyntaxReceiver is not QueryFinder receiver) return;

			foreach (var query in receiver.Queries)
				sb.Append(query.Type + query.ReversedSteps.Aggregate("", (current, next) => current + ", " + next.Type) + '\n');

			sb.Length--;
			
			sb.Append(@""";
	}
}");
			
			context.AddSource("Queries.cs", sb.ToString());
		}
	}
}