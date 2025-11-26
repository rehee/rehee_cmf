using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Entities
{
  public interface ISelect
  {
    string SelectValue { get; set; }
    string SelectKey { get; set; }
    bool SelectDisplay { get; set; }
  }
}
