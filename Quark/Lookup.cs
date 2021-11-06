using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Quark
{
	// System.Linq :|
	[SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
	[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
	public struct Lookup<TK, TE> : ILookup<TK, TE>
	{
		public IEnumerator<IGrouping<TK, TE>> GetEnumerator()
		{
			foreach (var pair in _elems)
				yield return new Grouping<TK, TE>(pair.Key, pair.Value);
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public bool Contains(TK key) => _elems.ContainsKey(key);

		public int Count => _elems.Keys.Count();

		public IEnumerable<TE> this[TK  key] => _elems[key];

		private readonly IReadOnlyDictionary<TK, IList<TE>> _elems;
		
		public Lookup(IReadOnlyDictionary<TK, IList<TE>> elems) => _elems = elems;

		public static Lookup<TK, TE> Create<TIn>(IList<TIn> list, Func<TIn, TK> keySel, Func<TIn, TE> elemSel)
		{
			var working = new Dictionary<TK, IList<TE>>();
			for (var i = 0; i < list.Count; i++)
			{
				var k = keySel(list[i]);
				if (!working.ContainsKey(k))
					working[k] = new List<TE> { elemSel(list[i]) };
				else
				{
					var dictL = working[k];
					dictL.Add(elemSel(list[i]));
					working[k] = dictL;
				}
			}

			return new Lookup<TK, TE>(working);
		}
	}
}