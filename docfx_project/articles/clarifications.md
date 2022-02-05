# Weird things in LINQ clarified

Want the complex stuff in LINQ explaining as easy as possible? Join making your head spin? Wondering *just what on earth AsEnumerable is for*?

You're in the right place!!!

## What are `AsEnumerable` & `AsList` for?
When defining your Enumerables, you can define whatever custom properties on it you want.

This includes creating methods with the same name as LINQ methods, such as `Where` or `Select`.

LINQ solves this with [`AsEnumerable`](xref:Quark.Linq.AsEnumerable``1(IEnumerable{``0})),
and Quark solves this with [`AsList`](xref:Quark.Linq.AsList``1(IList{``0})).