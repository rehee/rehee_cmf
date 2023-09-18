using Microsoft.AspNetCore.Identity;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContextModule.Entities
{
  public class TenantIdentityRoleClaim : IdentityRoleClaim<string>, IWithTenant
  {
    public Guid? TenantID { get; set; }
  }
  [EntityChangeTracker<TenantIdentityRoleClaim>]
  public class TenantIdentityRoleClaimRoleHandler : EntityChangeHandler<TenantIdentityRoleClaim>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      entity.TenantID = context?.TenantID;
    }
  }
}
