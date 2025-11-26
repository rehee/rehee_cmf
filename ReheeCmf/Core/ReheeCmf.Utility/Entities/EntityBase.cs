using ReheeCmf.Attributes;
using ReheeCmf.Tenants;

namespace ReheeCmf.Entities
{
	public abstract class EntityBase : IEntityBase, IWithTenant
	{
		public Guid? TenantID { get; set; }
		public abstract string StringId();
		public abstract object ObjectId();
	}
	public class EntityBase<T> : EntityBase, IEntityBase<T> where T : IEquatable<T>
	{
		public override object ObjectId()
		{
			return Id;
		}
		public override string StringId()
		{
			if (Id == null)
			{
				return "";
			}
			return Id.StringValue(typeof(T)) ?? "";
		}
		[FormInputs]
		[IgnoreUpdate]
		public virtual T Id { get; set; }
	}
}
