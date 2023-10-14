using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Entities;
using ReheeCmf.Handlers.EntityChangeHandlers;
using ReheeCmf.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.EntityHandlers
{
  [EntityChangeTracker<CmsEntity>]
  public class CmsEntityHandler : EntityChangeHandler<CmsEntity>
  {
    public override async Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      await Task.CompletedTask;
      var result = new List<ValidationResult>();
      if (entity?.CmsEntityMetadataId.HasValue is not true)
      {
        result.Add(ValidationResultHelper.New("CmsEntityMetadataId is required", nameof(CmsEntity.CmsEntityMetadataId)));
      }
      return result;
    }
    public override Task BeforeCreateAsync(CancellationToken ct = default)
    {
      return base.BeforeCreateAsync(ct);
    }
    public override Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      return base.BeforeUpdateAsync(propertyChange, ct);
    }
  }
}
