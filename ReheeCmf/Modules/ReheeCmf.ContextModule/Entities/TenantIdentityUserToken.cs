using Microsoft.OData.ModelBuilder;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.ODatas.Components;

namespace ReheeCmf.ContextModule.Entities
{
	public class TenantIdentityUserToken : IdentityUserToken<string>, IWithTenant
	{
		public Guid? TenantID { get; set; }
	}
	[EntityChangeTracker<TenantIdentityUserToken>]
	public class TenantIdentityUserTokenHandler : EntityChangeHandler<TenantIdentityUserToken>
	{
		public override void BeforeCreate()
		{
			base.BeforeCreate();
			entity?.TenantID = context?.TenantID;
		}
	}
	[ODataEntitySet<TenantIdentityUserToken>]
	public class TenantIdentityUserTokenSetHandler : ODataEntitySetHandler<TenantIdentityUserToken>
	{
		public override EntityTypeConfiguration<T> GetConfiguration<T>(ODataConventionModelBuilder builder)
		{
			return base.GetConfiguration<T>(builder).HasKey(b => b.Value);
		}

	}
}
