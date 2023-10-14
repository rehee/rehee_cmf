using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.Interfaces
{
  public interface IWithCmsQuery
  {
    bool? AsSplitQuery { get; set; }
    string? SelectedProperties { get; set; }
    bool? QueryBeforeFilter { get; set; }
    bool? HideProperty { get; set; }
  }
}
