using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons
{
  public class ServiceModuleOption
  {
    public bool IsDapr { get; set; }
    public string? ServiceName { get; set; }
    public string? BaseUrl { get; set; }
  }
}
