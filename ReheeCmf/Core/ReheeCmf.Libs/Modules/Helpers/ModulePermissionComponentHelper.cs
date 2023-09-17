using ReheeCmf.Modules.Components;
using ReheeCmf.Modules.Permissions;
using ReheeCmf.Utility.CmfRegisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Modules.Helpers
{
  public static class ModulePermissionComponentHelper
  {
    public static IEnumerable<string>? GetPermission<T>() where T : IModulePermission
    {
      return CmfRegister.ComponentPool.Values
        .Where(b => b is IModulePermissionComponent)
        .Where(b => b.EntityType == typeof(T))
        .SelectMany(b => (b as IModulePermissionComponent)!.GetSingletonPermissionHandler()!.ModulePermissions)
        .Distinct();

    }
  }
}
