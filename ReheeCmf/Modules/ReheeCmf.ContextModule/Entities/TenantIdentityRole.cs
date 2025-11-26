using ReheeCmf.Attributes;

namespace ReheeCmf.ContextModule.Entities
{
	public class TenantIdentityRole : IdentityRole, IWithTenant
	{
		public Guid? TenantID { get; set; }
	}

	[EntityChangeTracker<TenantIdentityRole>]
	public class TenantIdentityRoleHandler : EntityChangeHandler<TenantIdentityRole>
	{
		public override void BeforeCreate()
		{
			base.BeforeCreate();
			entity?.TenantID = context?.TenantID;
		}
	}
}
