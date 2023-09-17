using ReheeCmf.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReheeCmf.ContextModule.Entities
{
  public class ReheeCmfBaseUser : IdentityUser, ICmfUser
  {
    [NotMapped]
    public string[]? ImpersonateRoles { get; set; }
    [FormInputs(InputType = EnumInputType.File)]
    public string? Avatar { get; set; }


    public Guid? TenantID { get; set; }

    public virtual ICollection<TenantIdentityUserClaim> Claims { get; set; }
  }
}
