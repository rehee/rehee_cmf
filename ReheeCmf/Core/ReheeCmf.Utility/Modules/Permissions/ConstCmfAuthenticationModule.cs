using ReheeCmf.Modules.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Modules.Permissions
{
  [ModulePermission]
  public class ConstCmfAuthenticationModule : IModulePermission
  {
    public const string ReadRoleBasedAccess = "PermissionReadRoleBasedAccess";
    public const string UpdateRoleBasedAccess = "PermissionUpdateRoleBasedAccess";
  }


}
