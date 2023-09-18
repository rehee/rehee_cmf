using Microsoft.AspNetCore.Identity;
using Microsoft.OData.ModelBuilder;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.ODatas.Components;
using ReheeCmf.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContextModule.Entities
{
  public class TenantIdentityUserRole : IdentityUserRole<string>, IWithTenant
  {
    public Guid? TenantID { get; set; }
  }
  [EntityChangeTracker<TenantIdentityUserRole>]
  public class TenantIdentityUserRoleHandler : EntityChangeHandler<TenantIdentityUserRole>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      entity.TenantID = context?.TenantID;
    }
  }

  [ODataEntitySet<TenantIdentityUserRole>]
  public class TenantIdentityUserRoleSetHandler : ODataEntitySetHandler<TenantIdentityUserRole>
  {
    public override EntityTypeConfiguration<T> GetConfiguration<T>(ODataConventionModelBuilder builder)
    {
      return base.GetConfiguration<T>(builder).HasKey(b => b.RoleId);
    }
  }
}
