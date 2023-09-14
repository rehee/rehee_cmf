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
  public class TenantIdentityUserRole : IdentityUserRole<string>, IWithTenant
  {
    public Guid? TenantID { get; set; }
  }
  [ODataEntitySet<TenantIdentityUserRole>]
  public class TenantIdentityUserRoleSetHandler : ODataEntitySetHandler<TenantIdentityUserRole>
  {
    public override object EntitySet(ODataConventionModelBuilder builder)
    {
      return builder.EntitySet<TenantIdentityUserRole>(nameof(TenantIdentityUserRole)).EntityType.HasKey(b => b.RoleId);
    }
  }
}
