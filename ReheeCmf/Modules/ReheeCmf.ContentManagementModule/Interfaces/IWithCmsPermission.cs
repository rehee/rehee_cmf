using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.Interfaces
{
  public interface IWithCmsPermission
  {
    string? PermissionCreate { get; set; }
    string? PermissionRead { get; set; }
    string? PermissionUpdate { get; set; }
    string? PermissionDelete { get; set; }
  }
}
