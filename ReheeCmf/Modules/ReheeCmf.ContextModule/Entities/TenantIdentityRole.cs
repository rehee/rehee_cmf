using ReheeCmf.Components.ChangeComponents;

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
			if (entity != null)
			{
				entity.TenantID = context?.TenantID;
			}
		}
	}
}
