using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.DTOs
{
  public class ChangePasswordDTO : IValidatableObject
  {
    [Required]
    public string? OldPassword { get; set; }
    [Required]
    public string? NewPassword { get; set; }
    [Required]
    public string? ConfirmPassword { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (!String.Equals(NewPassword, ConfirmPassword))
      {
        yield return ValidationResultHelper.New("Password and confirm are not same", nameof(NewPassword), nameof(ConfirmPassword));
      }
    }
  }
}
