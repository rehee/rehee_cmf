using Microsoft.AspNetCore.Identity;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Tenants;

namespace ReheeCmf.ContextModule.Entities
{
  public class TenantIdentityRole : IdentityRole, IWithTenant
  {
    public Guid? TenantID { get; set; }
  }

  [EntityChangeTracker<TenantIdentityRole>]
  public class TenantIdentityRoleHandler : EntityChangeHandler<TenantIdentityRole>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      entity.TenantID = context?.TenantID;
    }
  }
}
