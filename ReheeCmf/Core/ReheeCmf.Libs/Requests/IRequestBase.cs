using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Requests
{
  public interface IRequestBase
  {
    void SetBaseToken(string token);
    void SetTenant(Guid? tenantId);
    void SetClientUrl(string url);
    void OverrideBaseUrl(string url);
    
    Task<ContentResponse<T>> Request<T>(
      HttpMethod method, string url, string? json = null, Stream? content = null,
      Dictionary<string, string>? headValue = null,
      string? name = null, string? token = null, CancellationToken ct = default);
  }
}
