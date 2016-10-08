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
public void GetMembers()
{
    var derived = new Derived();
    var value1 = derived.GetPrivate("_private");
    var value2 = derived.GetPrivate("_privateStatic");
    // ...
}

[TestMethod]
public void SetMembers()
{
    var derived = new Derived();
    var value1 = derived.SetPrivate("_private", "changed");
    var value2 = derived.SetPrivate("_privateStatic", "changed");
    // ...
}
```

How to Use
---
Simply refer PrivateObjectExtensions project from your unit test project or copy PrivateObjectExtensions.cs to your unit test project.

Why PrivateObjectExtensions?
---
PrivateObject doesn't allow you to access if the member is declared in base type unless you specify that type in constructor like this:
```csharp
// this works
var targetType = // find the type declaring the member somehow
var po = new PrivateObject(yourObject, new PrivateType(typeof(taragetType));
po.GetField("_private");
```
Additionally, if you want to access static member, you have to use PrivateType instead of PrivateObject like this:
```csharp
var pt = new PrivateType(yourObject.GetType());
pt.GetStaticField("_privateStatic");
```
But these are totally useless works. What we want to do is just accessing private member simply regardless of it's real type or static.

PrivateObjectExtensions automatically find proper way to access the member. No need to concern about them all!
