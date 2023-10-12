using ReheeCmf.Entities;

namespace ReheeCmf.ContentManagementModule.Entities
{
  public class CmsEntity : EntityBase<Guid>
  {
    public Guid? CmsEntityMetadataId { get; set; }
    public virtual CmsEntityMetadata? Metadata { get; set; }
    public virtual List<CmsProperty>? Properties { get; set; }
  }
}
