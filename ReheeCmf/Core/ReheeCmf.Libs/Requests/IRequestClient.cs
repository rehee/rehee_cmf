using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Requests
{
  public interface IRequestClient : IRequestBase
  {
    Task<ContentResponse<T>> Request<T>(
      HttpMethod method, string url, string? json = null, Stream? content = null,
      Dictionary<string, string>? headValue = null, CancellationToken ct = default);
  }
  public interface IRequestClient<T> : IRequestClient
  {
  }
}
