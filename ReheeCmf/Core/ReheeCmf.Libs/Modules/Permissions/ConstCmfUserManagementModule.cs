using ReheeCmf.Modules.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Modules.Permissions
{
  [ModulePermission]
  public class ConstCmfUserManagementModule : IModulePermission
  {
    public const string PermissionReadUsers = "ConstCmfUserManagementModule_PermissionReadUsers";
    public const string PermissionCreateUser = "ConstCmfUserManagementModule_PermissionCreateUser";
    public const string PermissionEditUser = "ConstCmfUserManagementModule_PermissionEditUser";
    public const string PermissionReadRolePermission = "ConstCmfUserManagementModule_PermissionReadRolePermission";
    public const string PermissionCreateRole = "ConstCmfUserManagementModule_PermissionCreateRole";
    public const string PermissionDeleteRole = "ConstCmfUserManagementModule_PermissionDeleteRole";
    public const string PermissionResetUserPassword = "ConstCmfUserManagementModule_PermissionResetUserPassword";
    public const string PermissionReadUserRolePermission = "ConstCmfUserManagementModule_PermissionReadUserRolePermission";

  }


}
