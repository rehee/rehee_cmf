using Microsoft.EntityFrameworkCore;
using ReheeCmf.Attributes;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.ContextModule.Entities;
using ReheeCmf.Entities;
using ReheeCmf.Handlers.EntityChangeHandlers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmfDemo.Data
{
	public class ApplicationDbContext : CmfIdentityContext<ReheeCmfBaseUser>
	{
		public ApplicationDbContext(IServiceProvider sp) : base(sp)
		{

		}


		public DbSet<Entity1> Entity1s { get; set; }

		public DbSet<EntityType1> EntityType1s { get; set; }
		public DbSet<EntityType2> EntityType2s { get; set; }
	}
	//, IWithName
	public class Entity1 : EntityBase<int>
	{
		public string? Name1 { get; set; }
		public string? Name2 { get; set; }
		[NotMapped]
		public DateTime? Date1 { get; set; }

		[ReadCheck]
		public static ReadCheck<Entity1> Entity1ReadCheck = (user) => b => b.Id > 20;
	}

	[EntityChangeTracker<Entity1>]
	public class Entity1Tracker : EntityChangeHandler<Entity1>
	{
		public override void BeforeCreate()
		{
			base.BeforeCreate();
			if (entity != null)
			{
				entity.Name1 = Guid.NewGuid().ToString();
			}
			//StatusException.Throw(System.Net.HttpStatusCode.BadRequest, "already with 3 chars");
		}
		public override IEnumerable<ValidationResult> Validation()
		{
			return [new ValidationResult("1", ["2"])];
		}
	}
	[EntityChangeTracker<EntityType1>]
	public class EntityType1Tracker : EntityChangeHandler<EntityType1>
	{
		public override void BeforeCreate()
		{
			base.BeforeCreate();
		}
		public override void BeforeDelete()
		{
			base.BeforeDelete();
			foreach (var sub in entity?.EntityType2s ?? [])
			{
				context?.Delete(sub);
			}
		}

		public override Task AfterCreateAsync(CancellationToken ct = default)
		{
			return base.AfterCreateAsync(ct);
		}
	}

	public class EntityType1 : EntityBase<int>
	{
		public string? Name { get; set; }
		public virtual List<EntityType2>? EntityType2s { get; set; }

	}
	public class EntityType2 : EntityBase<int>
	{
		public string? Name { get; set; }
		[ForeignKey(nameof(EntityType1))]
		public int? EntityType1Id { get; set; }
		public virtual EntityType1? EntityType1Entity { get; set; }
	}
}
