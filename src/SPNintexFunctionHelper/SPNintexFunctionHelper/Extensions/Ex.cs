using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SPNintexFunctionHelper.Extensions
{
  internal static class Ex
  {
    internal static IEnumerable<MethodInfo> GetPublicStaticMethods(this Type t)
    {
      return t.GetMethods(BindingFlags.Public | BindingFlags.Static);
    }

    internal static IEnumerable<T> GetCustomAttributes<T>(this MethodInfo mi)
    {
      return mi.GetCustomAttributes(typeof(T), false).Cast<T>();
    }
  }
}
