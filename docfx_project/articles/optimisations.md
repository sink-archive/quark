# Optimisations in Quark

## Enumerables vs Collections?

.NET's built-in LINQ provider, `System.Linq.Enumerable` is built around lazy-evaluated enumerables.
The inputs are not required to provide the elements in a random access form,
and there is no requirement for the length of the sequence.

While this is convenient, it has huge performance, complexity and possibly logic drawbacks.
For example, the overhead of querying is applied to every element each time,
as evaluating one element leads to evaluating every step of the query all at once, but for only one element.
This means we can't use optimised in-place algorithms, nor more efficient index access over enumeration.

In addition, the lack of requirement to provide a length means you can pass in enumerators that return elements infinitely.
Doing this will make looping over the linq results loop forever.
Passing this enumerable into some query functions (such as `ToArray` or `Aggregate`) will cause the thread to freeze,
eating CPU until the program runs out of RAM to store the query results,
with the garbage collector likely working overtime to keep everything under relative control.

Quark requires most query inputs to implement `IList<T>` (though there are some exceptions).
This means that:
  - we can use indexing, which is much faster than `IEnumerable.MoveNext()` + `IEnumerable.Current`.
  - we can perform in-place operations requiring the length of the array, such as reversing a list by swapping pairs
    * thanks to the above, *we can use quick sort...*

## `for` vs `foreach`?

Quark ***never* uses foreach**. Why? foreach uses enumerables.

That's right, foreach is syntactic sugar for enumerating:
```cs
// foreach syntax
foreach (var item in collection)
    doThing(item);

// equivalent syntax - identical once compiled
while (collection.MoveNext())
{
    var item = collection.Current;
    doThing(item);
}
```

In comparison, for loops use in-place mutability and indexing, and are therefore much faster.
While we haven't benchmarked it, however one user on StackOverflow claimed they found a 2x speedup with for over foreach!

## Sorting algorithms

Sorting is where Quark tends to pick up the most performance marks over System.Linq.

In `System.Linq.Enumerable.OrderBy`, a custom `OrderedEnumerable` is used to sort, which does actually use a quick sort, however its not ideal.
Quark in-place sorts on an array, which is very fast, whereas
[OrderedEnumerable](https://github.com/dotnet/runtime/blob/57bfe474518ab5b7cfe6bf7424a79ce3af9d6657/src/libraries/System.Linq/src/System/Linq/OrderedEnumerable.cs#L348)
does so much weirdness I couldn't actually figure out much of what theyre doing, but it appears to be sorting some sort of a map of indexes,
then returning enumerable results using that.

Whatever its doing, Quark's plain simple quick sort beats it. We currently use the Hoare partition scheme,
and while recursion mitigation optimisations are not made as of writing this, it may be done in the future.