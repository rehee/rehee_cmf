using ReheeCmf.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Entities
{
  public class RoleBasedPermission : EntityBase<Guid>
  {
    public string? ModuleName { get; set; }
    public string? RoleName { get; set; }
    public string? Permissions { get; set; }
    [IgnoreUpdate]
    [IgnoreMapping]
    public string? NormalizationModuleName { get; set; }

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

  [EntityChangeTracker<RoleBasedPermission, RoleBasedPermissionChangeHandler>(Group = nameof(RoleBasedPermission))]
  public class RoleBasedPermissionChangeHandler : EntityChangeHandler<RoleBasedPermission>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      if (entity!.Id == Guid.Empty)
      {
        entity.Id = Guid.NewGuid();
      }
      entity!.NormalizationModuleName = entity!.ModuleName?.Trim().ToUpper() ?? "";
      entity!.NormalizationRoleName = entity!.RoleName?.Trim().ToUpper() ?? "";
    }
    public override async Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      await base.BeforeUpdateAsync(propertyChange, ct);
      if (propertyChange.Any(b => b.PropertyName == nameof(entity.RoleName)))
      {
        entity!.NormalizationRoleName = entity!.RoleName?.Trim().ToUpper() ?? "";
      }
    }
    public override async Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      var existing = await base.ValidationAsync(ct);
      var result = new List<ValidationResult>();
      if (String.IsNullOrEmpty(entity!.NormalizationRoleName))
      {
        result.Add(ValidationResultHelper.New("RoleName is Required", nameof(RoleBasedPermission.RoleName)));
      }
      if (String.IsNullOrEmpty(entity!.NormalizationRoleName))
      {
        result.Add(ValidationResultHelper.New("Module is Required", nameof(RoleBasedPermission.ModuleName)));
      }
      if (result.Count <= 0)
      {
        if (context!.Query<RoleBasedPermission>(true)
          .Where(b => 
            !Guid.Equals(b.Id, entity.Id) && 
            String.Equals(b.NormalizationRoleName, entity.NormalizationRoleName) &&
            String.Equals(b.NormalizationModuleName, entity.NormalizationModuleName)).Select(b => b.Id).Any() == true)
        {
          result.Add(ValidationResultHelper.New("Permission with role and module already existing", nameof(RoleBasedPermission.RoleName)));
        };
      }


      return existing.Concat(result);
    }

  }

}
