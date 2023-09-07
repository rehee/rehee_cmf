using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Attributes
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class FindCheckAttribute : Attribute
  {
  }
  public delegate Func<T, bool> FindCheck<T>(TokenDTO user);
}
