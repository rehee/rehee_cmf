using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.StandardInputs.Properties
{
  internal interface IProperty
  {
    string? PropertyName { get; set; }
    string? Value { get; set; }
  }
}
