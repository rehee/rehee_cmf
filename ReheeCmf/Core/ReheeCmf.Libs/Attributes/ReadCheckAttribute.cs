using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Attributes
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class ReadCheckAttribute : Attribute
  {
  }
  public delegate Expression<Func<T, bool>> ReadCheck<T>(TokenDTO user);
}
