using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Quark
{
	[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
	[SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
	public struct Grouping<TK, TE> : IGrouping<TK, TE>
	{
		private readonly TE[] _elems;
		public Grouping(TK key, TE[] elems)
		{
			_elems = elems;
			Key    = key;
		}

		public Grouping(TK key, IList<TE> elems)
		{
			_elems = elems.ToArray();
			Key    = key;
		}

		public IEnumerator<TE> GetEnumerator()
		{
			// this is necessary as T[].GetEnumerator() is not generic
			for (var i = 0; i < _elems.Length; i++)
				yield return _elems[i];
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public TK Key { get; }
	}
}