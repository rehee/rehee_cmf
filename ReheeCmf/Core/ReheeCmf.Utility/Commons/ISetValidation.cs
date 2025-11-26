using System.ComponentModel.DataAnnotations;

namespace ReheeCmf.Commons
{
  public interface ISetValidation : IIsvalidate
  {
    void SetValidation(params ValidationResult[] validations);
  }
}
