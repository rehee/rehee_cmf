using ReheeCmf.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Entities
{
  [EntityChangeHandler<RoleBasedPermissionChangeHandler>]
  public class RoleBasedPermission : EntityBase<Guid>
  {
    public string? ModuleName { get; set; }
    public string? RoleName { get; set; }
    public string? Permissions { get; set; }

    [IgnoreUpdate]
    [IgnoreMapping]
    public string? NormalizationRoleName { get; set; }

    [NotMapped]
    public string[] PermissionList
    {
      get
      {
        var result = Permissions.ToIEnumerable();
        return result.ToArray();
      }
      set
      {
        Permissions = value.BackToString();
      }
    }
  }

  public class RoleBasedPermissionChangeHandler : EntityChangeHandler<RoleBasedPermission>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      if (entity!.Id == Guid.Empty)
      {
        entity.Id = Guid.NewGuid();
      }
      entity!.NormalizationRoleName = entity!.RoleName?.ToUpper() ?? "";
    }
    public override async Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      await base.BeforeUpdateAsync(propertyChange, ct);
      if (propertyChange.Any(b => b.PropertyName == nameof(entity.RoleName)))
      {
        entity!.NormalizationRoleName = entity!.RoleName?.ToUpper() ?? "";
      }
    }
  }

}
