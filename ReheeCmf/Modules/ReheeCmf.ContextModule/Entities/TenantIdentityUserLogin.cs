using Microsoft.OData.ModelBuilder;
using ReheeCmf.Attributes;
using ReheeCmf.ODatas.Components;

namespace ReheeCmf.ContextModule.Entities
{
	public class TenantIdentityUserLogin : IdentityUserLogin<string>, IWithTenant
	{
		public Guid? TenantID { get; set; }
	}

	[EntityChangeTracker<TenantIdentityUserLogin>]
	public class TenantIdentityUserLoginRoleHandler : EntityChangeHandler<TenantIdentityUserLogin>
	{
		public override void BeforeCreate()
		{
			base.BeforeCreate();
			entity?.TenantID = context?.TenantID;
		}
	}

	[ODataEntitySet<TenantIdentityUserLogin>]
	public class TenantIdentityUserLoginHandler : ODataEntitySetHandler<TenantIdentityUserLogin>
	{
		public override EntityTypeConfiguration<T> GetConfiguration<T>(ODataConventionModelBuilder builder)
		{
			return base.GetConfiguration<T>(builder).HasKey(b => b.LoginProvider);
		}
	}
}
