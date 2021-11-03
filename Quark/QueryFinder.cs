using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

		public IImmutableDictionary<SyntaxTree, List<InvocationExpressionSyntax>> Invocations
			=> _invocations.ToImmutableDictionary();

		private readonly Dictionary<SyntaxTree, List<InvocationExpressionSyntax>> _invocations = new();

		public IReadOnlyList<LinkedList<QueryStep>> Queries
			=> _invocations.Values.SelectMany(l => l.Select(ParseInvocation)).ToImmutableList();

		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			if (syntaxNode is not InvocationExpressionSyntax invSyntax) return;
			
			if (!_invocations.ContainsKey(invSyntax.SyntaxTree))
				_invocations[invSyntax.SyntaxTree] = new List<InvocationExpressionSyntax>();
			
			if (IsUsefulInvocation(invSyntax))
				_invocations[invSyntax.SyntaxTree].Add(invSyntax);
		}

		// ReSharper disable once SuggestBaseTypeForParameter
		private static string GetMethodName(InvocationExpressionSyntax invSyntax) => invSyntax.Expression switch
		{
			MemberAccessExpressionSyntax mae => mae.Name.ToFullString(),
			// invalid type, or i messed up
			_ => throw new ArgumentOutOfRangeException()
		};

		private static bool IsUsefulInvocation(InvocationExpressionSyntax invSyntax)
			=> UsefulInvocations.Contains(GetMethodName(invSyntax));

		// oh boy
		public static LinkedList<QueryStep> ParseInvocation(InvocationExpressionSyntax invocation)
		{
			var args = invocation.ArgumentList.Arguments;
			var name = GetMethodName(invocation);

			var queryStepType = Utils.ParseQueryStepType(name);

			var thisQueryStep = new QueryStep(queryStepType);
			
			// recursion base case
			if (invocation.Expression is MemberAccessExpressionSyntax { Expression: not InvocationExpressionSyntax })
				return new LinkedList<QueryStep>(new[] { thisQueryStep });

			if (invocation.Expression is not InvocationExpressionSyntax subInvocation)
				throw new ArgumentException($"sub-invocation was of type {invocation.Expression.GetType().FullName}",
											nameof(invocation));
			
			// recurse
			var subParsed = ParseInvocation(subInvocation);
			subParsed.AddFirst(thisQueryStep);

			return subParsed;
		}
	}
}