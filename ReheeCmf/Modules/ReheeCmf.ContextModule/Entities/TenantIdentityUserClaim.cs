using ReheeCmf.Attributes;
using ReheeCmf.Handlers.ChangeHandlers.EntityChangeHandlers;

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
			entity?.TenantID = context?.TenantID;
		}
	}
}
