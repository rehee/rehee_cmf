using ReheeCmf.Requests;
using ReheeCmf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Services
{
  public class ServiceModuleRequestFactory : IServiceModuleRequestFactory
  {
    private readonly IGetHttpClient factory;
    private readonly IServiceProvider sp;
    private readonly ApiSetting setting;
    private readonly IServiceModuleMapping mapping;
    private readonly IToken<IServiceModuleRequestFactory> token;
    private readonly IContextScope<Tenant> contextScopeTenant;

    public bool FlagOnly => true;

    public ServiceModuleRequestFactory(IGetHttpClient factory, IServiceProvider sp, ApiSetting setting, IServiceModuleMapping mapping, IToken<IServiceModuleRequestFactory> token, IContextScope<Tenant> contextScopeTenant)
    {
      this.factory = factory;
      this.sp = sp;
      this.setting = setting;
      this.mapping = mapping;
      this.token = token;
      this.contextScopeTenant = contextScopeTenant;
    }
    public RequestClient<IServiceModuleRequestFactory> GetRequest(string serviceName)
    {
      var key = mapping.Mapping.Keys.FirstOrDefault(b => String.Equals(b, serviceName, StringComparison.OrdinalIgnoreCase));
      var hasService = mapping.Mapping.TryGetValue(key ?? serviceName, out var func);
      var result = new RequestClient<IServiceModuleRequestFactory>(factory,
        sp.GetService<IGetRequestTokenService>()!
        , contextScopeTenant);


      StatusException.Throw(System.Net.HttpStatusCode.Ambiguous);
      return result;
    }
  }
}
