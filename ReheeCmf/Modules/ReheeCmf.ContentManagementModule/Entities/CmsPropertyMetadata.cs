using ReheeCmf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.Entities
{
  public class CmsPropertyMetadata : EntityBase<Guid>
  {
    public Guid? CmsEntityMetadataId { get; set; }
    public virtual CmsEntityMetadata? Metadata { get; set; }
    public virtual List<CmsProperty>? Properties { get; set; }
    public string? PropertyName { get; set; }
    public TypeCode PropertyType { get; set; }
  }
}
