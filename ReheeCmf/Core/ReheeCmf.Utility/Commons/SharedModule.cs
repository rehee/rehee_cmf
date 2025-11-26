using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons
{
  public class SharedModule
  {
    public string? ModuleName { get; set; }
    public string? Title { get; set; }
    public IEnumerable<string>? Permissions { get; set; }
  }
}
