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
  public class TenantIdentityUserLogin : IdentityUserLogin<string>, IWithTenant
  {
    public Guid? TenantID { get; set; }
  }

  [EntityChangeTracker<TenantIdentityUserLogin>]
  public class TenantIdentityUserLoginRoleHandler : EntityChangeHandler<TenantIdentityUserLogin>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      entity.TenantID = context?.TenantID;
    }
  }

  [ODataEntitySet<TenantIdentityUserLogin>]
  public class TenantIdentityUserLoginHandler : ODataEntitySetHandler<TenantIdentityUserLogin>
  {
    public override EntityTypeConfiguration<T> GetConfiguration<T>(ODataConventionModelBuilder builder)
    {
      return base.GetConfiguration<T>(builder).HasKey(b => b.LoginProvider);
    }
  }
}
