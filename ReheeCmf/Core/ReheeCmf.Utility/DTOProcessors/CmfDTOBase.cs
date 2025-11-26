using ReheeCmf.Helpers;

namespace ReheeCmf.DTOProcessors
{
	public abstract class CmfDTOBase<T> : IQueryKey, IValidatableObject, IQueryId<T>
	{
		public string? QueryKey { get; set; }
		public T? Id { get; set; }
		public virtual EnumIdType IdType => EnumIdTypeHelper.GetIdType<T>();

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}
	}
}
