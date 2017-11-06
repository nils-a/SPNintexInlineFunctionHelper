using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nintex.Workflow.Common;
using SPNintexFunctionHelperTests.TestFunctions;
using FluentAssertions;
using SPNintexFunctionHelper;

namespace SPNintexFunctionHelperTests
{
  [TestClass]
  public class RegistrationTests
  {
    [TestCleanup]
    public void Cleanup()
    {
      // unregister ALL
      NintexInlineFunctionAttribute.UnregisterAllFrom(GetType().Assembly);
    }

    [TestMethod]
    public void RegistrationByTypeWorksInGeneral()
    {
      // really ensure not-registered
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .Count(x => x.FunctionAlias == KlassOne.Names.DoSomething.FunctionAlias)
        .Should()
        .Be(0, "Something is broken...");

      // register new.
      NintexInlineFunctionAttribute.RegisterAllFrom(typeof(KlassOne));

      // assert
      var func = StringFunctionInfoCollection.GetInlineFunctions(-1)
        .FirstOrDefault(x => x.FunctionAlias == KlassOne.Names.DoSomething.FunctionAlias);
      func.Should().NotBeNull();
    }

    [TestMethod]
    public void RegistrationByAssemblyWorksInGeneral()
    {
      // really ensure not-registered
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .Count(x => x.FunctionAlias == KlassOne.Names.DoSomething.FunctionAlias)
        .Should()
        .Be(0, "Something is broken...");
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .Count(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias)
        .Should()
        .Be(0, "Something is broken...");

      // register new.
      NintexInlineFunctionAttribute.RegisterAllFrom(typeof(KlassOne).Assembly);

      // assert
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .FirstOrDefault(x => x.FunctionAlias == KlassOne.Names.DoSomething.FunctionAlias)
        .Should().NotBeNull();
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .FirstOrDefault(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias)
        .Should().NotBeNull();
    }

    [TestMethod]
    public void UnregistrationWorksInGeneral()
    {
      // really ensure not-registered
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .Count(x => x.FunctionAlias == KlassOne.Names.DoSomething.FunctionAlias)
        .Should()
        .Be(0, "Something is broken...");

      // register new.
      NintexInlineFunctionAttribute.RegisterAllFrom(typeof(KlassOne));

      // assert - one 
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .Count(x => x.FunctionAlias == KlassOne.Names.DoSomething.FunctionAlias)
        .Should()
        .Be(1, "Registration failed");

      // unregister 
      NintexInlineFunctionAttribute.UnregisterAllFrom(typeof(KlassOne));

      // assert - two
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .Count(x => x.FunctionAlias == KlassOne.Names.DoSomething.FunctionAlias)
        .Should()
        .Be(0, "Unregistration failed");
    }

    [TestMethod]
    public void TranslationWorkInGeneral()
    {
      // really ensure not-registered
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .Count(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias)
        .Should()
        .Be(0, "Something is broken...");

      // register new.
      NintexInlineFunctionAttribute.RegisterAllFrom(typeof(KlassTwo));

      // assert - one 
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .Single(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias)
        .Lcid
        .Should()
        .Be(-1, "Registration failed for no lcid");
      StringFunctionInfoCollection.GetInlineFunctions(KlassTwo.Names.DoSomething.LcidOne)
        .Single(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias)
        .Lcid.Should()
        .Be(KlassTwo.Names.DoSomething.LcidOne, "Registration failed for lcid de-de");
      StringFunctionInfoCollection.GetInlineFunctions(KlassTwo.Names.DoSomething.LcidTwo)
        .Single(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias)
        .Lcid.Should()
        .Be(KlassTwo.Names.DoSomething.LcidTwo, "Registration failed for lcid de-at");
      StringFunctionInfoCollection.GetInlineFunctions(1032)
        .Single(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias)
        .Lcid.Should()
        .Be(-1, "Registration is broken for non-existing lcid");
    }

    [TestMethod]
    public void UsageGetTranslatedOrCopied()
    {
      // really ensure not-registered
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .Count(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias)
        .Should()
        .Be(0, "Something is broken...");

      // register new.
      NintexInlineFunctionAttribute.RegisterAllFrom(typeof(KlassTwo));

      // assert - description is localized 
      var de = StringFunctionInfoCollection.GetInlineFunctions(KlassTwo.Names.DoSomething.LcidOne)
        .Single(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias);
      de.Description.Should().BeEquivalentTo(KlassTwo.Names.DoSomething.DescriptionOne);
      // assert - description is copied
      var at = StringFunctionInfoCollection.GetInlineFunctions(KlassTwo.Names.DoSomething.LcidTwo)
        .Single(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias);
      at.Description.Should().BeEquivalentTo(KlassTwo.Names.DoSomething.Description);
    }

    [TestMethod]
    public void DescriptionGetTranslatedOrCopied()
    {
      // really ensure not-registered
      StringFunctionInfoCollection.GetInlineFunctions(-1)
        .Count(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias)
        .Should()
        .Be(0, "Something is broken...");

      // register new.
      NintexInlineFunctionAttribute.RegisterAllFrom(typeof(KlassTwo));

      // assert - usage is copied
      var en = StringFunctionInfoCollection.GetInlineFunctions(-1)
        .Single(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias);
      var de = StringFunctionInfoCollection.GetInlineFunctions(KlassTwo.Names.DoSomething.LcidOne)
        .Single(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias);
      de.Usage.Should().BeEquivalentTo(en.Usage);
      // assert - usage is localized
      var at = StringFunctionInfoCollection.GetInlineFunctions(KlassTwo.Names.DoSomething.LcidTwo)
        .Single(x => x.FunctionAlias == KlassTwo.Names.DoSomething.FunctionAlias);
      at.Usage.Should().BeEquivalentTo(KlassTwo.Names.DoSomething.UsageTwo);
    }
  }
}
