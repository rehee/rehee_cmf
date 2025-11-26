using ReheeCmf.Attributes;
using ReheeCmf.Handlers.ChangeHandlers.EntityChangeHandlers;

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
			entity?.TenantID = context?.TenantID;
		}
	}
}
