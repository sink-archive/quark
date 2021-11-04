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
			context.AddSource("Util.cs", @"
using System;
using System.Text;
namespace Quark.Linq
{
	internal static class Util
	{
		/// <summary>
		/// Gets a unique string for the content of the delegate method.
		/// The same string is returned for the same CIL code.
		/// </summary>
		// to those looking thru Generator.cs: see Utils.cs for the equivalent of this method used in the generator 
		internal static string GetILStr(this Delegate @delegate)
			=> Encoding.UTF8.GetString(@delegate.Method.GetMethodBody()?.GetILAsByteArray() ?? Array.Empty<byte>());
	}
}");
			
			var sb = new StringBuilder(@"
using System;
namespace Quark.Linq
{
	public static class Queries
	{
		public static Tout[] Select<Tin, Tout>(this Tin[] input, Func<Tin, Tout> func) => new Tout[0];
		public static T[] Where<T>(this T[] input, Func<T, bool> func) => new T[0];
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