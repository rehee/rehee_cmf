using Microsoft.AspNetCore.Identity;
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
}
