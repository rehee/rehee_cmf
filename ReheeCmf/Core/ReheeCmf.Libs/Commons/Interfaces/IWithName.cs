using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.Interfaces
{
  public interface IWithName
  {
    string? Name1 { get; set; }
    void Change();
  }
}
