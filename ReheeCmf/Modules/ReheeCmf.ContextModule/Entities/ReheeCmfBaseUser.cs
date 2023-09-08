using Microsoft.AspNetCore.Identity;
using ReheeCmf.Entities;
using ReheeCmf.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContextModule.Entities
{
  public class ReheeCmfBaseUser : IdentityUser, ICmfUser
  {
    public string[]? ImpersonateRoles { get; set; }
    [FormInputs(InputType = EnumInputType.File)]
    public string? Avatar { get; set; }


    public Guid? TenantID { get; set; }
  }
}
