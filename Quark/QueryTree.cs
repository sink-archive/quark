using System;
using System.Collections.Generic;

namespace Quark
{
	internal class QueryTree<T>
	{
		public List<QueryTreeNode<T>> FirstLevelNodes = new();

		public static QueryTree<QueryStep> QueryStepsToTree(QueryEnd[] queries)
			=> throw new NotImplementedException("oh boy here we go");
	}

	internal class QueryTreeNode<T>
	{
		public T                      Key;
		// please for the love of god let this store as a reference so my ram dont go brrrrrrr and i dont need pointers
		public QueryTreeNode<T>?      Parent;
		public List<QueryTreeNode<T>> Children = new();

		public QueryTreeNode(T key, QueryTreeNode<T>? parent = null)
		{
			Key    = key;
			Parent = parent;
		}
	}
}