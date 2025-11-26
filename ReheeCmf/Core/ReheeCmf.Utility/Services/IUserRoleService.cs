using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Services
{
  public interface IUserRoleService
  {
    Task UserRoleAsync(object user, Dictionary<string, string?> properities, CancellationToken ct = default);
  }
}
