using SPNintexFunctionHelper;

namespace SPNintexFunctionHelperTests.TestFunctions
{
  public class KlassOne
  {
    public static class Names
    {
      // ReSharper disable once MemberHidesStaticFromOuterClass
      public static class DoSomething
      {
        public const string FunctionAlias = "fn-doSomething";
        public const string Description = "some cool description of "+ FunctionAlias;
      }
      public static class DoAnotherThing
      {
        public const string FunctionAlias = "fn-doAnotherThing";
        public const string Description = "some cool description of " + FunctionAlias;
        public const string Usage = "this is my custom usage...";
      }
    }

    [NintexInlineFunction(Names.DoSomething.FunctionAlias,
      Description = Names.DoSomething.Description)]
    public static string DoSomething()
    {
      return string.Empty;
    }

    [NintexInlineFunction(Names.DoAnotherThing.FunctionAlias,
      Description = Names.DoAnotherThing.Description,
      Usage = Names.DoAnotherThing.Usage)]
    public static string DoAnotherThing()
    {
      return string.Empty;
    }

  }
}
