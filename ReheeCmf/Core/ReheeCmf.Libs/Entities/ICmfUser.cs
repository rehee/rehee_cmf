using ReheeCmf.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Entities
{
  public interface IAvatar
  {
    string? Avatar { get; set; }
  }

  public interface IUserName
  {
    string UserName { get; set; }
  }
  public interface ICmfUser : IId<string>, IAvatar, IUserName, IWithTenant
  {
    string NormalizedUserName { get; set; }
    string Email { get; set; }
    string NormalizedEmail { get; set; }
    bool EmailConfirmed { get; set; }
    bool LockoutEnabled { get; set; }
    DateTimeOffset? LockoutEnd { get; set; }

    string[]? ImpersonateRoles { get; set; }
  }

}
