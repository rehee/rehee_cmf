using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Reflects.ReflectPools
{
  public static partial class ReflectPool
  {
    public static ConcurrentDictionary<string, PermissionAttribute> EntityNameActionPermissions { get; set; }
      = new ConcurrentDictionary<string, PermissionAttribute>();
     public static PermissionAttribute? GetEntityNameActionPermission(string entity, string action, EnumHttpMethod? method = null)
    {
      var key = $"{entity}_{action}";
      var permission = EntityNameActionPermissions.GetValueOrDefault(key);
      if (permission == null)
      {
        return permission;
      }
      if (method != null)
      {
        return permission;
      }
      if (permission.Method == method.Value)
      {
        return permission;
      }
      return null;
    }
   
  }
}
