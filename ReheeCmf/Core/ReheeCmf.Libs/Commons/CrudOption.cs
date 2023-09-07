using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons
{
  public class CrudOption : IUserAndRole
  {
    public string EntityQueryUri { get; set; }
    public string DefaultReadOnlyConnectionString { get; set; }
    public bool DisableUseLazyLoadingProxies { get; set; }
    public EnumSQLType? SQLType { get; set; }

    public string DefaultConnectionString { get; set; }
    //reserved api endpoint
    public const string DataApiEndpoint = "Api/Data";
    public const string UserManagementApiEndpoint = "Api/Users";
    public const string EntityQueryEndpoint = "Api/Data";
    public const string DTOProcessor = "Api/Dto";

    public const string UserEndpoint = "Read";
    public const string RoleEndpoint = "Roles";
    public const string UserRoleEndpoint = "UserRoles";


    //public const string AdminEntityQueryEndpoint = "Admin/Api/Data/Read";

    public const string DataEndpoint = "{entityName}/{key?}";
    public const string ReadItemEndpoint = "Item/{entityName}/{key?}";

    public static string DataUrl(string entityName, string key, bool isItem = false)
    {
      return $"{DataApiEndpoint}{(isItem ? "/Item" : "")}/{entityName}/{(String.IsNullOrWhiteSpace(key) ? "-1" : key)}";
    }

    public string? EntityQueryRouterName { get; set; } = "Data";


    public string? EntityQueryUrl2 { get; set; }

    //config for Entiy List Grid
    public int EntityGridResultPerPage { get; set; } = 10;


    //EntityName for system generic type.
    public string EntityKey_Permision { get; set; } = "Permision`1";
    public string EntityKey_IdentityUserRole { get; set; } = "TenantIdentityUserRole";
    public string EntityKey_IdentityRole { get; set; } = "IdentityRole";
    public string EntityKey_IdentityRoleClaim { get; set; } = "IdentityRoleClaim`1";
    public string EntityKey_IdentityUser { get; set; } = "IdentityUser";
    public string EntityKey_IdentityUserToken { get; set; } = "IdentityUserToken`1";
    public Type? UserType { get; set; }
    public Type? RoleType { get; set; }

    public const string Read = "read";
    public const string Create = "create";
    public const string Update = "update";
    public const string Delete = "delete";

    public const string EntityName_User = "User_Entity";
    public const string EntityName_Role = "Role_Entity";
    public const string EntityName_IdentityUserRole = "TenantIdentityUserRole";

    public const string UserChangePasswordEndpoint = "ChangePassword";
    public const string UserChangeAvatarEndpoint = "ChangeAvatar";
    public const string ResetUserPasswordEndpoint = "ResetUserPassword";
    public const string ForgotPasswordEndpoint = "ForgotPassword";
  }
}
