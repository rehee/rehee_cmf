using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Services
{
  public interface IAdditionalTokenService<T>
  {
    Task<Dictionary<string, string>?> GetAdditionalDTOClaimAsync(T user, CancellationToken ct = default);
    Task<IEnumerable<Claim>?> GetAdditionalClaimAsync(T user, CancellationToken ct = default);
    Task<Dictionary<string, string>?> GetAdditionalDTOClaimAsync(IEnumerable<Claim> userClaims, CancellationToken ct = default);
  }
}
