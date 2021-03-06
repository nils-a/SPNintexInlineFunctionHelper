﻿using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Nintex.Workflow.Common;
using SPNintexFunctionHelper.Extensions;

namespace SPNintexFunctionHelper
{
  [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
  public class NintexInlineFunctionAttribute: Attribute
  {
    /// <summary>
    /// Register all Nintex inline-fuctions from all exported (public) types in the assembly
    /// </summary>
    public static void RegisterAllFrom(Assembly assembly)
    {
      assembly.GetExportedTypes().ToList().ForEach(RegisterAllFrom);
    }

    /// <summary>
    /// Register all Nintex inline-fuctions in this type
    /// </summary>
    public static void RegisterAllFrom(Type type)
    {
      foreach (var method in type.GetPublicStaticMethods())
      {
        foreach (var attrib in method.GetCustomAttributes<NintexInlineFunctionAttribute>())
        {
          attrib.Register(type, method);
        }
      }
    }

    /// <summary>
    /// Unregister all Nintex inline-fuctions from all exported (public) types in the assembly
    /// </summary>
    public static void UnregisterAllFrom(Assembly assembly)
    {
      assembly.GetExportedTypes().ToList().ForEach(UnregisterAllFrom);
    }

    /// <summary>
    /// Unregister all Nintex inline-fuctions in this type
    /// </summary>
    public static void UnregisterAllFrom(Type type)
    {
      foreach (var method in type.GetPublicStaticMethods())
      {
        foreach (var attrib in method.GetCustomAttributes<NintexInlineFunctionAttribute>())
        {
          attrib.Unregister();
        }
      }
    }

    /// Set this to something like "fn-myFunction"
    /// A string that represents the name of the function alias for the inline function. The name of the function alias is used to invoke the inline function in workflow actions.
    /// The maximum length for this parameter is 512 characters.
    /// Note: Nintex suggests that function aliases for inline functions use 15 characters or less, if possible.
    public NintexInlineFunctionAttribute(string functionAlias)
    {
      FunctionAlias = functionAlias;
    }

    private string FunctionAlias { get; set; }

    /// <summary>
    /// Optional. A string that represents the description of the function alias.
    /// The maximum length for this parameter is 500 characters.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Optional. A string that represents the description of the syntax information for the function alias.
    /// The maximum length for this parameter is 500 characters.
    /// If left empty - a "nice" value will be auto-generated by this attribute....
    /// </summary>
    public string Usage { get; set; }

    private void Unregister()
    {
      StringFunctionInfoCollection.Remove(FunctionAlias);
    }

    private void Register(Type type, MethodInfo method)
    {
      var infos = StringFunctionInfoCollection.Instance;
      var existing = infos.FirstOrDefault(f => f.FunctionAlias == FunctionAlias);
      if(existing != null)
      {
        if (existing.TypeName == type.Name && existing.AssemblyName == type.Assembly.FullName)
        {
          //this is "ours"...
          StringFunctionInfoCollection.Remove(FunctionAlias);
        }
        else
        {
          throw new ArgumentException(string.Format("An InlineFunction with the function alias \"{0}\" already exists!", FunctionAlias));
        }
      }

      StringFunctionInfoCollection.Add(FunctionAlias, type.Assembly.FullName, type.Namespace, type.Name, method.Name, Description, GetUsageOrDefault(type, method), -1, false);

      foreach (var translation in method.GetCustomAttributes<NintexInlineFunctionLocalizationAttribute>())
      {
        var description = translation.Description;
        if (string.IsNullOrEmpty(description))
        {
          description = Description;
        }
        var usage = translation.Usage;
        if (string.IsNullOrEmpty(usage))
        {
          usage = GetUsageOrDefault(type, method);
        }
        StringFunctionInfoCollection.Add(FunctionAlias, type.Assembly.FullName, type.Namespace, type.Name, method.Name, description, usage, translation.Lcid, false);
      }
    }

    private string GetUsageOrDefault(Type type, MethodInfo method)
    {
      if (!string.IsNullOrEmpty(Usage))
      {
        return Usage;
      }

      var overloads = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(x => x.Name == method.Name);
      var first = true;
      var sb = new StringBuilder();
      foreach (var overload in overloads)
      {
        if (first)
        {
          first = false;
        }
        else
        {
          sb.Append(", \n");
        }

        sb.Append(FunctionAlias + "(");
        sb.Append(string.Join(", ", overload.GetParameters().Select(p => string.Format("{0} {1}", p.ParameterType.Name, p.Name)).ToArray()));
        sb.Append(")");
    
      }

      return sb.ToString();
    }
  }
}
