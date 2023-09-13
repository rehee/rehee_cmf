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
  public class TenantIdentityUserToken : IdentityUserToken<string>, IWithTenant
  {
    public Guid? TenantID { get; set; }
  }

  [ODataEntitySet<TenantIdentityUserToken, TenantIdentityUserTokenSetHandler>]
  public class TenantIdentityUserTokenSetHandler : ODataEntitySetHandler<TenantIdentityUserToken>
  {
    public override object EntitySet(ODataConventionModelBuilder builder)
    {
      return builder.EntitySet<TenantIdentityUserToken>(nameof(TenantIdentityUserToken)).EntityType.HasKey(b => b.Value);
    }
  }
}
