using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Requests
{
  public interface IGetHttpClient
  {
    HttpClient GetClient(string name);
  }
}
