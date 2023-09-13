using ReheeCmf.Handlers.ChangeHandlers;
using System;
using System.Net;

namespace ReheeCmf.Handlers.EntityChangeHandlers
{
  public abstract class EntityChangeHandler<T> : ChangeHandler<T>, IEntityChangeHandler where T : class
  {
    public override async Task SetTenant(CancellationToken ct = default)
    {
      await base.SetTenant(ct);
      var service = sp!.GetService<IEntityTenantService<T>>();
      var tEntity = (entity as IWithTenant)!;
      if (service != null)
      {
        tEntity.TenantID = service.GetTenant(context, scopedUser?.Value);
      }
      if (tEntity.TenantID == null)
      {
        tEntity.TenantID = context?.TenantID;
      }
    }

  }
}
