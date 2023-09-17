using ReheeCmf.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Modules.Components
{
  public interface IModulePermissionComponent : ICmfComponent
  {
    IModulePermissionHandler GetSingletonPermissionHandler();
  }
  public class ModulePermissionAttribute : CmfComponentAttribute, IModulePermissionComponent, IEntityComponent
  {
    public override Type? HandlerType => typeof(ModulePermissionAttribute);

    private IModulePermissionHandler? handler { get; set; }
    public IModulePermissionHandler GetSingletonPermissionHandler()
    {
      if (handler == null)
      {
        handler = new ModulePermissionHandler(EntityType!);
      }
      return handler;
    }
  }

  public interface IModulePermissionHandler : ICmfHandler
  {
    string[] ModulePermissions { get; }
  }

  public class ModulePermissionHandler : IModulePermissionHandler
  {
    public ModulePermissionHandler(Type permissionModuleType)
    {

      ModulePermissions = permissionModuleType.GetAllFields(true).Where(b => b.FieldType == typeof(string))
        .Select(b =>
        {
          if (b.GetValue(null) is string str)
          {
            return str;
          }
          return "";
        }).ToArray();
    }

    public string[] ModulePermissions { get; }
  }
}
