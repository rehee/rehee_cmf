using ReheeCmf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.Entities
{
  public class CmsProperty : EntityBase<Guid>
  {
    public Guid? CmsEntityId { get; set; }
    public virtual CmsEntity? Entity { get; set; }

    public Guid? CmsPropertyMetadataId { get; set; }
    public virtual CmsPropertyMetadata? Property { get; set; }


    public string? ValueString { get; set; }
  }
}
