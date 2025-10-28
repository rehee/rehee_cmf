using Microsoft.OData.ModelBuilder;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.ODatas.Components;

namespace ReheeCmf.ContextModule.Entities
{
	public class TenantIdentityUserRole : IdentityUserRole<string>, IWithTenant
	{
		public Guid? TenantID { get; set; }
	}
	[EntityChangeTracker<TenantIdentityUserRole>]
	public class TenantIdentityUserRoleHandler : EntityChangeHandler<TenantIdentityUserRole>
	{
		public override void BeforeCreate()
		{
			base.BeforeCreate();
			entity?.TenantID = context?.TenantID;
		}
	}

	[ODataEntitySet<TenantIdentityUserRole>]
	public class TenantIdentityUserRoleSetHandler : ODataEntitySetHandler<TenantIdentityUserRole>
	{
		public override EntityTypeConfiguration<T> GetConfiguration<T>(ODataConventionModelBuilder builder)
		{
			return base.GetConfiguration<T>(builder).HasKey(b => b.RoleId);
		}
	}
}
