using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Services
{
  public interface ITenantImpersonateService<TUser> where TUser : ICmfUser
  {
    Task<bool> CheckImpersonateAsync(TUser user);
    Task<TUser?> GetImpersonateUserAsync(string userName, string? password = null);
  }
}
