using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Authenticates
{
  public class UserManagementOption
  {
    public static UserManagementOption Detault => new UserManagementOption()
    {
      RoleProperty = "Roles",
      RoleLabel = "Roles",
      RoleIsMulti = true,
      UserDetailPropertyCreate = new string[] { "UserName", "Email", "Password", "PhoneNumber" },
      UserDetailPropertyEdit = new string[] {   "PhoneNumber", "EmailConfirmed", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount" }
    };
    public const string PasswordProperty = "Password";
    public static (string PropertyName, EnumInputType InputType, bool Editable)[] UserInputs =
      new (string PropertyName, EnumInputType InputType, bool Editable)[]
      {
        ("Id",EnumInputType.Text,false),
        ("UserName",EnumInputType.Text,false),
        ("Email",EnumInputType.Email,false),
        ("Password",EnumInputType.Password,false),
        ("PhoneNumber",EnumInputType.Text,true),
        ("EmailConfirmed",EnumInputType.Switch,true),
        ("PhoneNumberConfirmed",EnumInputType.Switch,true),
        ("TwoFactorEnabled",EnumInputType.Switch,false),
        ("LockoutEnd",EnumInputType.DateTime,true),
        ("LockoutEnabled",EnumInputType.Switch,true),
        ("AccessFailedCount",EnumInputType.Number,true),
      };
    public string? RoleProperty { get; set; }
    public string? RoleLabel { get; set; }
    public bool RoleIsMulti { get; set; }

    public bool ResetPasswordEncrypt { get; set; }
    public string[]? UserDetailPropertyCreate { get; set; }
    public string[]? UserDetailPropertyEdit { get; set; }

  }

}
