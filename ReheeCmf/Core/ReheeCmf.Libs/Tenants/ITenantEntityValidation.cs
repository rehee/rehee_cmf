namespace ReheeCmf.Tenants
{
	public interface ITenantEntityValidation
	{
		IEnumerable<ValidationResult> Validate(TenantEntity tenant, IContext context);
	}
}
