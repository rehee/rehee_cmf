using Dapr.Client;
using ReheeCmf.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Services
{
  public class ServiceModuleMapping : IServiceModuleMapping
  {
    public ServiceModuleMapping(ApiSetting apiSetting, IHttpClientFactory factory)
    {
      Mapping = new Dictionary<string, Func<HttpClient>>();
      if (apiSetting?.ServiceOptions?.Any() == true)
      {
        foreach (var o in apiSetting.ServiceOptions.Where(b => !string.IsNullOrEmpty(b.ServiceName))
          .DistinctBy(b => b.ServiceName).Where(b => b != null && !String.IsNullOrEmpty(b.ServiceName)))
        {
          Mapping.Add(o.ServiceName!, () =>
          {
            if (o.IsDapr)
            {
              return DaprClient.CreateInvokeHttpClient(o.ServiceName);
            }
            else
            {
              var client = factory.CreateClient(o.ServiceName!);
              if (!String.IsNullOrEmpty(o.BaseUrl))
              {
                client.BaseAddress = new Uri(o.BaseUrl);
              }
              return client;
            }
          });
        }
      }
    }
    public Dictionary<string, Func<HttpClient>> Mapping { get; set; }
  }
}
