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
  [EntityChangeTracker<CmsEntityMetadata>]
  public class CmsEntityMetadataHandler : EntityChangeHandler<CmsEntityMetadata>
  {
    public override async Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      var validationResult = new List<ValidationResult>();
      entity!.EntityNameNormalization = entity.EntityName?.ToUpper();
      if (await context!.Query<CmsEntityMetadata>(true).AnyAsync(
        b => String.Equals(b.EntityNameNormalization, entity.EntityNameNormalization) && b.Id != entity.Id) == true)
      {
        validationResult.Add(ValidationResultHelper.New("An entity already existing", nameof(entity.Entities)));
      }
      return validationResult;
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
