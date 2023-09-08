using Microsoft.AspNetCore.Identity;
using ReheeCmf.Tenants;

namespace ReheeCmf.ContextModule.Entities
{
  public class TenantIdentityRole : IdentityRole, IWithTenant
  {
    public Guid? TenantID { get; set; }
  }
}
