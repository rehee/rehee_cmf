using ReheeCmf.Components.ChangeComponents;

namespace ReheeCmf.ContextModule.Entities
{
	public class TenantIdentityUserClaim : IdentityUserClaim<string>, IWithTenant
	{
		public Guid? TenantID { get; set; }

	}
	[EntityChangeTracker<TenantIdentityUserClaim>]
	public class TenantIdentityUserClaimRoleHandler : EntityChangeHandler<TenantIdentityUserClaim>
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
