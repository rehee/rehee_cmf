using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
  [EntityChangeTracker<CmsPropertyMetadata>]
  public class CmsPropertyMetadataHandler : EntityChangeHandler<CmsPropertyMetadata>
  {
    public override async Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      var result = new List<ValidationResult>();
      if (entity?.CmsEntityMetadataId.HasValue is not true)
      {
        result.Add(ValidationResultHelper.New("CmsEntityMetadataId is required", nameof(entity.CmsEntityMetadataId)));
      }
      entity!.PropertyNameNormalization = entity?.PropertyName?.ToUpper();
      if (await context!.Query<CmsPropertyMetadata>(true).AnyAsync(b =>
        b.PropertyNameNormalization == entity!.PropertyNameNormalization && b.CmsEntityMetadataId == entity!.CmsEntityMetadataId && b.Id != entity.Id, ct) == true)
      {
        result.Add(ValidationResultHelper.New("Property Name is Unique", nameof(entity.PropertyName)));
      }
      return result;
    }
    public override Task BeforeCreateAsync(CancellationToken ct = default)
    {
      return base.BeforeCreateAsync(ct);
    }
    public override async Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      await base.BeforeUpdateAsync(propertyChange, ct);
      if (propertyChange.Any(b => b.PropertyName == nameof(CmsPropertyMetadata.PropertyType) || b.PropertyName == nameof(CmsPropertyMetadata.PropertyName)))
      {
        if (entity?.Properties?.Any() == true)
        {
          foreach (var p in entity!.Properties!)
          {
            p.PropertyType = entity.PropertyType;
            p.PropertyName = entity.PropertyName;
            context?.TrackEntity(p);
          }
        }
      }
    }
  }
}
