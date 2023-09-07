using ReheeCmf.Requests;
using ReheeCmf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Services
{
  public class TokenFromContextScope : IToken<IServiceModuleRequestFactory>
  {
    private readonly IContextScope<TokenDTO> user;

    public TokenFromContextScope(IContextScope<TokenDTO> user)
    {
      this.user = user;
    }
    public Task<string?> GetTokenAsync(CancellationToken ct)
    {
      return Task.FromResult(
        user?.Value?.TokenString
        );
    }
  }
}
