using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Quark
{

	internal class QueryFinder : ISyntaxReceiver
	{
		/// <summary>
		/// The names of LINQ functions we care about: Functions that return a value other than an IEnumerable, plus the custom ToEnumerable
		/// </summary>
		private static readonly HashSet<string> UsefulInvocations = new(new[]
		{
			"Aggregate", "All", "Any", "Average", "Contains", "Count", "ElementAt", "ElementAtOrDefault", "First",
			"FirstOrDefault", "Last", "LastOrDefault", "LongCount", "Max", "Min", "SequenceEqual", "Single",
			"SingleOrDefault", "Sum", "ToArray", "ToDictionary", "ToHashSet", "ToList", "ToLookup", "ToEnumerable"
		});
		
		public Dictionary<SyntaxTree, List<InvocationExpressionSyntax>> Invocations { get; } = new();


		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			if (syntaxNode is not InvocationExpressionSyntax invSyntax) return;
			
			if (!Invocations.ContainsKey(invSyntax.SyntaxTree))
				Invocations[invSyntax.SyntaxTree] = new();
			if (IsUsefulInvocation(invSyntax))
				Invocations[invSyntax.SyntaxTree].Add(invSyntax);
		}

		// ReSharper disable once SuggestBaseTypeForParameter
		private static string? GetInvocationMethodName(InvocationExpressionSyntax invSyntax)
		{
			return invSyntax.Expression switch
			{
				// normal methods
				IdentifierNameSyntax idn => idn.Identifier.ToFullString(),
				// generic methods
				GenericNameSyntax gn     => gn.Identifier.ToFullString(),
				// extension methods
				MemberAccessExpressionSyntax mae => mae.Name.ToFullString(),
				// invalid type, or i messed up
				_ => throw new ArgumentOutOfRangeException()
			};
		}
		
		private static bool IsUsefulInvocation(InvocationExpressionSyntax invSyntax)
		{
			var methodName = GetInvocationMethodName(invSyntax);
			return methodName != null && UsefulInvocations.Contains(methodName);
		}

		// oh boy
		public static LinkedList<QueryStep> ParseInvocation(InvocationExpressionSyntax invocation)
			=> throw new NotImplementedException();
	}
}