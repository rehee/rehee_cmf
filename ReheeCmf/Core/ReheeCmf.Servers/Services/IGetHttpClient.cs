using ReheeCmf.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Services
{
  public class GetHttpClient : IGetHttpClient
  {
    private readonly IHttpClientFactory factory;

    public GetHttpClient(IHttpClientFactory factory)
    {
      this.factory = factory;
    }
    public HttpClient GetClient(string name)
    {
      return factory.CreateClient(name);
    }
  }
}
