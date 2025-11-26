using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Authenticates
{
  public class CmfAuthorizeAttribute : Attribute
  {
    public bool AuthOnly { get; set; } = false;
    public string EntityName { get; set; } = "entityName";
    public string EntityNameForce { get; set; }
    public string EntityKey { get; set; } = "key";
    public string EntityAction { get; set; }
    public bool EntityRoleBase { get; set; } = true;
    public string RoleString { get; set; }
    public bool PermissionClass { get; set; }
    public bool PermissionAction { get; set; }
  }
}
