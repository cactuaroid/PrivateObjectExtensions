PrivateObjectExtensions
---
PrivateObjectExtensions is one wrapper class providing extension methods for [PrivateObject](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.testtools.unittesting.privateobject.aspx). PrivateObjectExtensions allows you to
- get/set private member by simple extension methods
- even if the member is declared in base type

Sample
---
```csharp
public class Base
{
    private string _private = "private member";
    private static string _privateStatic = "private static member";
}

public class Derived : Base
{
}
```
```csharp
[TestMethod]
public void CanGetMembers()
{
    var derived = new Derived();
    var value1 = derived.GetPrivate("_private");
    var value2 = derived.GetPrivate("_privateStatic");
    // ...
}
```

How to Use
---
Simply refer PrivateObjectExtensions project from your unit test project or copy PrivateObjectExtensions.cs to your unit test project.
