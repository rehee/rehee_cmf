using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Requests
{
  public interface IServiceModuleMapping
  {
    Dictionary<string, Func<HttpClient>> Mapping { get; set; }
  }
}
