using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.DTOProcessors
{
  public abstract class CmfDTOBase : IQueryKey, IValidatableObject
  {
    public string? QueryKey { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      return Enumerable.Empty<ValidationResult>();
    }
  }
}
