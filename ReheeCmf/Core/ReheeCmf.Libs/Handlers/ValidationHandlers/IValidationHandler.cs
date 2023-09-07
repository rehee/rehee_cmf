using System.ComponentModel.DataAnnotations;

namespace ReheeCmf.Handlers.ValidationHandlers
{
  public interface IValidationHandler
  {
    Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default);
  }
}
