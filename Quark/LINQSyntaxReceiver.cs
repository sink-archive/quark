using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Quark
{

	internal class LinqSyntaxReceiver : ISyntaxReceiver
	{
		public List<ExpressionSyntax> Expressions { get; } = new();

		private static ExpressionSyntax? FindLinqExpression(SyntaxNode syntaxNode)
		{
			if (syntaxNode is ExpressionSyntax exprSyntax)
			{
				return exprSyntax;
			}

			return null;
		}


		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			var valid = FindLinqExpression(syntaxNode);
			if (valid != null)
				Expressions.Add(valid);
		}
	}
}