namespace ReheeCmf.ContentManagementModule.Registers
{

  [RegistrableEntity]
  public class ContentRegistrableEntity : IRegistrableEntity
  {
    public IEnumerable<Type> QueryableEntities => [
      typeof(CmsEntityMetadata),
      typeof(CmsPropertyMetadata),
      typeof(CmsEntity),
      typeof(CmsProperty),
      typeof(CmsContentDTO)
    ];

    public void RegisterEntity(object builder, ITenantContext db)
    {
      if (builder is ModelBuilder modelBuilder)
      {
        modelBuilder.Entity<CmsEntityMetadata>()
        .HasQueryFilter(b => db.IgnoreTenant || b.TenantID == db.TenantID);
        modelBuilder.Entity<CmsPropertyMetadata>()
          .HasQueryFilter(b => db.IgnoreTenant || b.TenantID == db.TenantID);
        modelBuilder.Entity<CmsEntity>()
          .HasQueryFilter(b => db.IgnoreTenant || b.TenantID == db.TenantID);
        modelBuilder.Entity<CmsEntity>()
        .Property(b => b.IsPublished)
        .HasComputedColumnSql(
          """
          CASE 
            WHEN [Status] = 2 THEN CAST(1 AS bit)
            WHEN [Status] = 4 AND [PublishedDate] IS NOT NULL AND [PublishedDate] >= CONVERT(datetimeoffset, SYSDATETIMEOFFSET()) AT TIME ZONE 'UTC' THEN CAST(1 AS bit)
            WHEN [Status] = 4 AND [UpPublishedDate] IS NOT NULL AND [UpPublishedDate] < CONVERT(datetimeoffset, SYSDATETIMEOFFSET()) AT TIME ZONE 'UTC' THEN CAST(0 AS bit)
            ELSE CAST(0 AS bit)
          END
          """);
        //.HasComputedColumnSql("CASE WHEN [LockoutEnd] IS NOT NULL AND [LockoutEnd] > SYSDATETIMEOFFSET() THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END");


        modelBuilder.Entity<CmsProperty>()
          .HasQueryFilter(b => db.IgnoreTenant || b.TenantID == db.TenantID);
      }
    }
  }
}
