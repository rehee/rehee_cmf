using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helper
{
  public static class ValidationResultHelper
  {
    public static ValidationResult New(string message, params string[] keys)
    {
      return new ValidationResult(message, keys);
    }
  }
}
