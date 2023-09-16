using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.MultiTenants
{
  public abstract class ServiceWithTenant : IServiceWithTenant
  {
    private readonly IContextScope<Tenant> scopeTenant;

    public ServiceWithTenant(IContextScope<Tenant> scopeTenant)
    {
      this.scopeTenant = scopeTenant;
      if (this.scopeTenant.Value != null)
      {
        SetTenantDetail(this.scopeTenant.Value);
      }
      this.scopeTenant.ValueChange += (a, b) =>
      {
        SetTenantDetail(b.Value);
      };
    }
    public Tenant? CurrentTenant { get; protected set; }
    public virtual void SetTenantDetail(Tenant? tenant)
    {
      this.CurrentTenant = tenant;
      if (delegates?.Any() == true)
      {
        this.tenantChange(this, new EventArgs<Tenant>() { Value = tenant });
      }
    }
    List<EventHandler<EventArgs<Tenant>>> delegates = new List<EventHandler<EventArgs<Tenant>>>();
    private event EventHandler<EventArgs<Tenant>> tenantChange = delegate { };
    public virtual event EventHandler<EventArgs<Tenant>> TenantChange
    {
      add
      {
        tenantChange += value;
        delegates.Add(value);
      }
      remove
      {
        tenantChange -= value;
        delegates.Remove(value);
      }

    }
    protected bool _disposed { get; set; }
    public void Dispose()
    {
      if (_disposed)
      {
        return;
      }
      _disposed = true;
      try
      {
        var list = delegates.Select(b => b).ToArray();
        foreach (var d in list)
        {
          TenantChange -= d;
        }
        delegates.Clear();
        list = null;
        GC.SuppressFinalize(this);
      }
      catch { }
    }
  }

}
