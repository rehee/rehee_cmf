using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Entities;
using ReheeCmf.Handlers.EntityChangeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.EntityHandlers
{
  [EntityChangeTracker<CmsBase>]
  public class CmsBaseHandler : EntityChangeHandler<CmsBase>
  {
    public override Task BeforeCreateAsync(CancellationToken ct = default)
    {
      if (entity is not null)
      {
        entity.CreateDate = DateTime.UtcNow;
        entity.CreateUserId = context?.User?.UserId;
      }
      return base.BeforeCreateAsync(ct);
    }
    public override Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      if (entity is not null)
      {
        entity.UpdateDate = DateTime.UtcNow;
        entity.UpdateUserId = context?.User?.UserId;
      }
      return base.BeforeUpdateAsync(propertyChange, ct);
    }
  }
}
