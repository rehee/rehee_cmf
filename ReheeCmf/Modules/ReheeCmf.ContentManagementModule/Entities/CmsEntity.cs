using ReheeCmf.ContentManagementModule.Interfaces;
using ReheeCmf.Entities;
using ReheeCmf.Enums;
using System.ComponentModel.DataAnnotations;

namespace ReheeCmf.ContentManagementModule.Entities
{
  public class CmsEntity : CmsBase, IWithCmsStatus
  {
    
    public Guid? CmsEntityMetadataId { get; set; }
    public virtual CmsEntityMetadata? Metadata { get; set; }
    public virtual List<CmsProperty>? Properties { get; set; }
    public EnumContentStatus? Status { get; set; }
    public DateTimeOffset? PublishedDate { get; set; }
    public DateTimeOffset? UpPublishedDate { get; set; }
    public bool? IsPublished { get; set; }
  }
}
