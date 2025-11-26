using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Services
{
  public interface IUserTokenStorage
  {
    Task<bool> AddOrUpdateTokenAsync(string userName, TokenDTO? token, CancellationToken ct = default);
    Task<TokenDTO?> GetUserTokenAsync(string userName, CancellationToken ct = default);

  }
}
