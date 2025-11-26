using ReheeCmf.Contexts;

namespace ReheeCmf.Services
{
	public interface IEntityTenantService<T> where T : class
	{
		Guid? GetTenant(IContext? context, TokenDTO? user);
	}
}
