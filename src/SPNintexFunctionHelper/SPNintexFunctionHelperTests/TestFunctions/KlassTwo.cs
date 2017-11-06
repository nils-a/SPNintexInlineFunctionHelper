using SPNintexFunctionHelper;

namespace SPNintexFunctionHelperTests.TestFunctions
{
  public class KlassTwo
  {
    public static class Names
    {
      // ReSharper disable once MemberHidesStaticFromOuterClass
      public static class DoSomething
      {
        public const string FunctionAlias = "fn-doSomething2";
        public const string Description = "some cool description of " + FunctionAlias;
        public const int LcidOne = 1031;
        public const int LcidTwo = 3079;
        public const string DescriptionOne = "Hier eine deutsche beschreibung";
        public const string UsageTwo = "This is the usage for LCID-TWO";
      }
    }

    [NintexInlineFunction(Names.DoSomething.FunctionAlias,
      Description = Names.DoSomething.Description),
    NintexInlineFunctionLocalization(Names.DoSomething.LcidOne,
      // no usage here - check it 
      Description = Names.DoSomething.DescriptionOne),
    NintexInlineFunctionLocalization(Names.DoSomething.LcidTwo,
      // no description here - check it has been copied
      Usage = Names.DoSomething.UsageTwo)]
    public static string DoSomething()
    {
      return string.Empty;
    }
  }
}