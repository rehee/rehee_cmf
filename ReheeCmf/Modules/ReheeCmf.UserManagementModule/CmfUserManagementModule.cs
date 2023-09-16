using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Commons;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Contexts;
using ReheeCmf.Helpers;
using ReheeCmf.Modules;
using ReheeCmf.ODatas.Commons;
using ReheeCmf.Services;
using ReheeCmf.UserManagementModule.Services;
using ReheeCmf.ODatas;
using ReheeCmf.ODatas.Helpers;
using ReheeCmf.ODatas.Components;
using Microsoft.OData.ModelBuilder;
using System.Xml.Linq;
namespace ReheeCmf.UserManagementModule
{
  public class CmfUserManagementModule<TUser, TRole, TUserRole> : ServiceModule
    where TUser : IdentityUser, new()
    where TRole : IdentityRole, new()
    where TUserRole : IdentityUserRole<string>, new()
  {
    public override string ModuleTitle => "CmfUserManagementModule";

    public override string ModuleName => "CmfUserManagementModule";

    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.ConfigureServicesAsync(context);
      context.Services!.AddScoped<IUserService, UserService<TUser, TRole, TUserRole>>();

    }
    public override async Task PostConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.PostConfigureServicesAsync(context);
      context.MvcBuilder.AddCmfOdataEndpoint(sp =>
      {
        var m = sp.GetEdmModelUser(
          builder =>
          {
            var ue = builder.GetType().GetMethod("EntitySet").MakeGenericMethod(typeof(TUser))
            .Invoke(builder, new object[] { CrudOption.UserEndpoint });
            var userEntrity = ue.GetPropertyValue("EntityType").Content;
            //builder.EntitySet<ReheeCmfBaseUser>("ReheeCmfBaseUser").EntityType.Property<bool>
            //ODataEntitySetExtensions.SetODataMapping(typeof(TUser), userEntrity);
            builder.EntitySet<TUserRole>(CrudOption.UserRoleEndpoint).EntityType.HasKey(b => b.RoleId);
            //builder.EntitySet<TUser>(CrudOption.UserEndpoint).EntityType.Ignore(b => b.PasswordHash);
            //builder.EntitySet<TUser>(CrudOption.UserEndpoint).EntityType.Ignore(b => b.SecurityStamp);
            //builder.EntitySet<TUser>(CrudOption.UserEndpoint).EntityType.Ignore(b => b.ConcurrencyStamp);
            //builder.EntitySet<TUser>(CrudOption.UserEndpoint).EntityType.Ignore(b => b.ConcurrencyStamp);
            ODataEntitySetFactory.GetHandler(typeof(TUser)).EntitySet(builder, CrudOption.Read);
            //if (context.BuilderUser?.Any() == true)
            //{
            //  foreach (var b in context.BuilderUser)
            //  {
            //    b(builder);
            //  }
            //}
          }, typeof(TUser), typeof(TRole));
        return m;
      }, CrudOption.UserManagementApiEndpoint, new ODataEndpointMapping[] {
          ODataEndpointMapping.New("UserManageController", "GetUser", "Read/{role?}", null, "Read"),
          ODataEndpointMapping.New("UserManageController", "GetUser", "{role?}", null, "Read"),
          ODataEndpointMapping.New("UserManageController", "GetRoles", "Roles", null, "Roles"),
          ODataEndpointMapping.New("UserManageController", "GetuserRoles", "UserRoles", null, "Read"),
      });
    }

    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      IEnumerable<string> permissions = new string[]
      {

      };
      return Task.FromResult(permissions);
    }
  }

  [ODataEntitySet<IdentityUser>]
  public class UserBaseODataSet : ODataEntitySetHandler<IdentityUser>
  {
    public override void ConfigurationEntitySet<T>(EntityTypeConfiguration<T> entitySet)
    {
      entitySet.Ignore(b => b.PasswordHash);
      entitySet.Ignore(b => b.SecurityStamp);
      entitySet.Ignore(b => b.ConcurrencyStamp);
      entitySet.Ignore(b => b.ConcurrencyStamp);
    }

  }
}
