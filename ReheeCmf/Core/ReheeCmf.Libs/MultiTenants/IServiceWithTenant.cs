using ReheeCmf.Commons;

namespace ReheeCmf.MultiTenants
{
	public interface IServiceWithTenant : IDisposable, ISetTenantDetail
	{
		Tenant? CurrentTenant { get; }
		event EventHandler<EventArgs<Tenant>> TenantChange;
	}
}
