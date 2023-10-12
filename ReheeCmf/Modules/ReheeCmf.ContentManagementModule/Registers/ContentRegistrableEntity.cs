using Microsoft.EntityFrameworkCore;

namespace ReheeCmf.ContentManagementModule.Registers
{
  [RegistrableEntity]
  public class ContentRegistrableEntity : IRegistrableEntity
  {
    public IEnumerable<Type> QueryableEntities => [
      typeof(CmsEntityMetadata),
      typeof(CmsPropertyMetadata),
      typeof(CmsEntity),
      typeof(CmsProperty)
    ];

    public void RegisterEntity(object builder, ITenantContext db)
    {
      if (builder is ModelBuilder != true)
      {
        return;
      }
      var modelBuilder = (builder as ModelBuilder)!;
      modelBuilder.Entity<CmsEntityMetadata>()
        .HasQueryFilter(b => db.IgnoreTenant || b.TenantID == db.TenantID);
      modelBuilder.Entity<CmsPropertyMetadata>()
        .HasQueryFilter(b => db.IgnoreTenant || b.TenantID == db.TenantID);
      modelBuilder.Entity<CmsEntity>()
        .HasQueryFilter(b => db.IgnoreTenant || b.TenantID == db.TenantID);
      modelBuilder.Entity<CmsProperty>()
        .HasQueryFilter(b => db.IgnoreTenant || b.TenantID == db.TenantID);
    }

  }
}
