using ReheeCmf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.Entities
{
  public abstract class CmsBase : EntityBase<Guid>
  {
    public DateTime? CreateDate { get; set; }
    public string? CreateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string? UpdateUserId { get; set; }
    
  }
}
