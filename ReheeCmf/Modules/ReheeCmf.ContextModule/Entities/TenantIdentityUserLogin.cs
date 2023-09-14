using Microsoft.AspNetCore.Identity;
using Microsoft.OData.ModelBuilder;
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
  [ODataEntitySet<TenantIdentityUserLogin>]
  public class TenantIdentityUserLoginHandler : ODataEntitySetHandler<TenantIdentityUserLogin>
  {
    public override object EntitySet(ODataConventionModelBuilder builder)
    {
      return builder.EntitySet<TenantIdentityUserLogin>(nameof(TenantIdentityUserLogin)).EntityType.HasKey(b => b.LoginProvider);
    }
  }
}
