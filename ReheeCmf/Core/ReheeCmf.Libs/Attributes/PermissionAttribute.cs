using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Attributes
{
  public class PermissionAttribute : Attribute
  {
    public PermissionAttribute()
    {

    }
    public PermissionAttribute(string permission, EnumHttpMethod method = EnumHttpMethod.NotSpecified)
    {
      Permission = permission;
      Method = method;
    }
    public PermissionAttribute(PermissionWithMethod[] permissions)
    {
      Permissions = permissions;
    }
    public string Permission { get; }
    public EnumHttpMethod Method { get; }
    public PermissionWithMethod[] Permissions { get; }
  }

  public class PermissionWithMethod
  {
    public PermissionWithMethod(string permission, EnumHttpMethod method)
    {
      Permission = permission;
      Method = method;
    }
    public string Permission { get; set; }
    public EnumHttpMethod Method { get; set; }
  }
}
