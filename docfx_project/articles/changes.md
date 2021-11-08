# Changes against LINQ in Quark

## `LongCount`
`LongCount` is not implemented.

## `IList<T>`
Quark uses `IList<T>` over `IEnumerable<T>` where possible to optimise for performance.
Some operations may work equally efficiently with more fundamental base types, and in those cases they will be preferred over list.
All functions return the most specialised type possible, usually `List<T>` or `T[]`

The choice of `IList<T>` is simple: it accounts for both arrays and lists, the two most common types of collection in .NET,
while retaining plenty of performance.

## `NonGeneric<T>` and `Cast<TIn, TOut>`
For technical reasons, an `IList<T>` cannot be implicitly converted to an `IList` the same way an `IEnumerable<T>` can be converted to an `IEnumerable`.
To counter this, we have added a new method to LINQ: [`NonGeneric<T>()`](xref:Quark.Linq.NonGeneric``1(IList{``0})).

This method takes a generic `IList<T>` and returns the equivalent nongeneric `List<T>`.

In addition, we added a version of [`Cast`](xref:Quark.Linq.Cast``2(IList{``0})) capable of taking a generic list,
with the drawback that you must re-specify the type of the list.
If the list type is specified wrong, the code will not compile.

The old type comes before the new type, so think of `Cast<int, double>()` as *"Cast from `int` to `double`"*.

```cs
var myList = new [] { 5, 7, 3, 7 };

// System.Linq code - won't compile in Quark
myList.Cast<double>().ToList();

// Quark code with NonGeneric
myList.NonGeneric().Cast<double>();

// Quark code with generic Cast
myList.Cast<int, double>();
```

## Extra additions

Some functions, such as [`GroupBy`](xref:Quark.Linq.GroupBy``2(IList{``0},Func{``0,``1})) have additional overloads,
this helps in some cases where you might pass basic defaults that really need not be specified in the query,
and make sense to abstract behind an overload.