using ReheeCmf.Components.ChangeComponents;

namespace ReheeCmf.ContextModule.Entities
{
	public class TenantIdentityRoleClaim : IdentityRoleClaim<string>, IWithTenant
	{
		public Guid? TenantID { get; set; }
	}
	[EntityChangeTracker<TenantIdentityRoleClaim>]
	public class TenantIdentityRoleClaimRoleHandler : EntityChangeHandler<TenantIdentityRoleClaim>
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
