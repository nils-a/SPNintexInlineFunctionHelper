# SPNintexInlineFunctionHelper
Helps registering nintex inline-functions i.e. for usage in SharePoint-Features

## Create a static function and give it a function-alias
Give it a function-alias and a description. Usage will be generated - this works if you're in lcid 1033
```csharp
[NintexInlineFunction("fn-doSomeCoolStuff",
    Description = "Does real cool stuff. Really!")]
public static string DoSomething()
{
    return string.Empty;
}
```

To make the function visible in a german (lcid:1031) web but without any translations:
```csharp
[NintexInlineFunction("fn-doSomeCoolStuff",
    Description = "Does real cool stuff. Really!"),
NintexInlineFunctionLocalization(1031)]
public static string DoSomething()
{
    return string.Empty;
}
```

To add a german translation:
```csharp
[NintexInlineFunction("fn-doSomeCoolStuff",
    Description = "Does real cool stuff. Really!"),
NintexInlineFunctionLocalization(1031,
    Description = "Wirklivh tolles zeug. Verprochen!")]
public static string DoSomething()
{
    return string.Empty;
}
```

## Registering & unregistering
```csharp
// register all functions in all public classes from "this" assembly
NintexInlineFunctionAttribute.RegisterAllFrom(this.GetType().Assembly);
// register functions from one class only
NintexInlineFunctionAttribute.RegisterAllFrom(typeof(TheClassFromTheOtherAssembly));
// unregister is the same, really!
NintexInlineFunctionAttribute.UnregisterAllFrom(typeof(SomeClassFromYetAnotherAssembly).Assembly);
```