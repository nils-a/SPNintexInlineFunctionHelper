using System;

namespace SPNintexFunctionHelper
{
  /// <summary>
  /// This is for translations of Nintex custom inline funcitons.
  /// DO NOT set LCID to 1033 (or every other english nintex-function is broken...)
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
  public class NintexInlineFunctionLocalizationAttribute : Attribute
  {
    public NintexInlineFunctionLocalizationAttribute(int lcid)
    {
      if (lcid < 1)
      {
        throw new ArgumentException("lcid must be a positive number", "lcid");
      }
      if (lcid == 1033)
      {
        throw new ArgumentException("setting lcid to 1033 is not supported", "lcid");        
      }

      Lcid = lcid;
    }

    public string Description { get; set; }
    public string Usage { get; set; }
    public int Lcid { get; private set; }
  }
}
