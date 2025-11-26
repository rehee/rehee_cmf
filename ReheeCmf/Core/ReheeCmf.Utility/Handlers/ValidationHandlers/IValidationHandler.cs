namespace ReheeCmf.Handlers.ValidationHandlers
{
	public interface IValidationHandler
	{
		IEnumerable<ValidationResult> Validation();
	}
}
