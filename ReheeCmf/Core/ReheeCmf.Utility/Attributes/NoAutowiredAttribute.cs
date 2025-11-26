using System;

namespace ReheeCmf.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class NoAutowiredAttribute : Attribute
  {
  }
}
