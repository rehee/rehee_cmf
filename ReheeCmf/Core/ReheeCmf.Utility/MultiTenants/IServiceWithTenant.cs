using ReheeCmf.Commons;
using ReheeCmf.Tenants;

namespace ReheeCmf.MultiTenants
{
	public interface IServiceWithTenant : IDisposable, ISetTenantDetail
	{
		Tenant? CurrentTenant { get; }
		event EventHandler<EventArgs<Tenant>> TenantChange;
	}
}
