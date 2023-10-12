using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json.Linq;
using ReheeCmf.ContentManagementModule.DTOs;
using ReheeCmf.ContextModule.Entities;
using ReheeCmf.ODatas.Components;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Runtime.Serialization;

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
      typeof(CmfContentDTO)
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
  public class MyCustomEntity
  {
    public Guid Id { get; set; }
    public Dictionary<string, object> Data { get; set; }
  }
}
