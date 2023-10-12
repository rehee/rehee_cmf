using ReheeCmf.Entities;

namespace ReheeCmf.ContentManagementModule.Entities
{
  public class CmsEntityMetadata : EntityBase<Guid>
  {
    public string? EntityName { get; set; }
    public virtual List<CmsPropertyMetadata>? Properities { get; set; }
    public virtual List<CmsEntity>? Entities { get; set; }
  }
}
