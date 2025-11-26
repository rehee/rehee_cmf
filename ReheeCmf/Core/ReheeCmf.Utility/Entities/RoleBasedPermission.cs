

using ReheeCmf.Attributes;
using ReheeCmf.Handlers.EntityChangeHandlers;

namespace ReheeCmf.Entities
{
	public class RoleBasedPermission : EntityBase<string>
	{
		public RoleBasedPermission()
		{
			Id = Guid.NewGuid().ToString();
		}
		public string? ModuleName { get; set; }
		public string? RoleName { get; set; }
		public string? Permissions { get; set; }
		public string? NormalizationModuleName { get; set; }
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

	[EntityChangeTracker<RoleBasedPermission>(Group = nameof(RoleBasedPermission))]
	public class RoleBasedPermissionChangeHandler : EntityChangeHandler<RoleBasedPermission>
	{
		public override void BeforeCreate()
		{
			base.BeforeCreate();
			entity?.NormalizationModuleName = entity?.ModuleName?.Trim().ToUpper() ?? "";
			entity?.NormalizationRoleName = entity?.RoleName?.Trim().ToUpper() ?? "";

		}
		public override void BeforeUpdate(EntityChanges[] propertyChange)
		{
			base.BeforeUpdate(propertyChange);
			if (propertyChange.Any(b => b.PropertyName == nameof(entity.RoleName)))
			{
				entity?.NormalizationRoleName = entity?.RoleName?.Trim().ToUpper() ?? "";
			}
		}
		public override IEnumerable<ValidationResult> Validation()
		{
			var existing = base.Validation();
			var result = new List<ValidationResult>();
			if (String.IsNullOrEmpty(entity!.NormalizationRoleName))
			{
				result.Add(ValidationResultHelper.New("RoleName is Required", nameof(RoleBasedPermission.RoleName)));
			}
			if (String.IsNullOrEmpty(entity!.NormalizationRoleName))
			{
				result.Add(ValidationResultHelper.New("Module is Required", nameof(RoleBasedPermission.ModuleName)));
			}



			return existing.Concat(result);
		}

	}

}
