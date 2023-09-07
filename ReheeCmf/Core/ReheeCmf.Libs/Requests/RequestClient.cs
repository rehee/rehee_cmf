using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Requests
{
  public class RequestClient : RequestBase, IRequestClient
  {
    protected readonly IGetHttpClient getHttpClient;
    private readonly IGetRequestTokenService clientTokenService;

    public RequestClient(IGetHttpClient getHttpClient, IGetRequestTokenService clientTokenService, IContextScope<Tenant> contextScopeTenant) : base(contextScopeTenant)
    {
      this.getHttpClient = getHttpClient;
      this.clientTokenService = clientTokenService;
    }

    protected override HttpClient GetHttpClient(string name = null)
    {
      return getHttpClient.GetClient(name);
    }
    public async Task<ContentResponse<T>> Request<T>(HttpMethod method, string url, string? json = null, Stream? content = null, Dictionary<string, string>? headValue = null, CancellationToken ct = default)
    {
      var tokens = await clientTokenService.GetRequestTokenAsync(ct);
      return await Request<T>(method, url, json, content, headValue, tokens.name, tokens.token, ct);
    }
  }
  public class RequestClient<T> : RequestClient, IRequestClient<T>
  {
    public RequestClient(IGetHttpClient getHttpClient, IGetRequestTokenService clientTokenService, IContextScope<Tenant> contextScopeTenant) : base(getHttpClient, clientTokenService, contextScopeTenant)
    {

    }
  }
}
