using ReheeCmf.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.Interfaces
{
  public interface IWithCmsStatus
  {
    EnumContentStatus? Status { get; set; }
    DateTimeOffset? PublishedDate { get; set; }
    DateTimeOffset? UpPublishedDate { get; set; }
    bool? IsPublished { get; set; }
  }
}
