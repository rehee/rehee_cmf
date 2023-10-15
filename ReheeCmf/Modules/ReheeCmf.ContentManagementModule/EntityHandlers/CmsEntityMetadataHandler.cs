using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Entities;
using ReheeCmf.Handlers.EntityChangeHandlers;
using ReheeCmf.Helpers;
using ReheeCmf.Storages;
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
    public override void Dispose()
    {
      base.Dispose();
      storage = null;
    }
    protected IStorage<CmsEntityMetadata, Guid>? storage { get; set; }
    public override void Init(IServiceProvider sp, object entity, int index, int subindex, string? group = null)
    {
      base.Init(sp, entity, index, subindex, group);
      storage = sp.GetService<IStorage<CmsEntityMetadata, Guid>>();
    }
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
    public override async Task AfterCreateAsync(CancellationToken ct = default)
    {
      await base.AfterCreateAsync(ct);
      await storage!.AfterCreatetOrUpdateAsync(entity, ct);
    }
    public override async Task AfterUpdateAsync(CancellationToken ct = default)
    {
      await base.AfterUpdateAsync(ct);
      await storage!.AfterCreatetOrUpdateAsync(entity, ct);
    }
    public override async Task AfterDeleteAsync(CancellationToken ct = default)
    {
      await base.AfterDeleteAsync(ct);
      await storage!.AfterDeletetAsync(entity, ct);
    }
  }
}
