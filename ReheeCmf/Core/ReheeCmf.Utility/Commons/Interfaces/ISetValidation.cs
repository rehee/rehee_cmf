namespace ReheeCmf.Commons
{
	public interface ISetValidation : IIsvalidate
	{
		void SetValidation(params ValidationResult[] validations);
	}
}
