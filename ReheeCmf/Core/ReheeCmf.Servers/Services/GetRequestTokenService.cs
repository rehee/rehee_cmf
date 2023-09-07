using ReheeCmf.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Services
{
  public class GetRequestTokenService : IGetRequestTokenService
  {
    private readonly IContextScope<TokenDTO> user;

    public GetRequestTokenService(IContextScope<TokenDTO> user)
    {
      this.user = user;
    }
    public Task<(string? name, string? token)> GetRequestTokenAsync(CancellationToken ct = default)
    {
      return Task.FromResult((
        user?.Value?.UserName,
        user?.Value?.TokenString));
    }
  }
}
