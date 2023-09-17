using Google.Api;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using ReheeCmf.Authenticates;
using ReheeCmf.ContextModule;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.ContextModule.Entities;
using ReheeCmf.ContextModule.Managers;
using ReheeCmf.ContextModule.Providers;
using ReheeCmf.Services;
using ReheeCmf.UserManagementModule.Services;

namespace ReheeCmf.Libs.Test.ContextsTest.GeneralTests
{
  internal class UserServiceTest : ContextsTest<CmfIdentityContext<ReheeCmfBaseUser>>
  {
    IServiceProvider sp { get; set; }
    [SetUp]
    public override void Setup()
    {

      base.Setup();
      sp = ConfigService(services =>
      {
        services.AddSingleton<TokenManagement>(sp => new TokenManagement());
        services.AddSingleton<ApiSetting>(sp => new ApiSetting());
        services.AddSingleton<ILogger<RoleManager<TenantIdentityRole>>>((new Mock<ILogger<RoleManager<TenantIdentityRole>>>()).Object);
        services.AddSingleton<ILogger<UserManager<ReheeCmfBaseUser>>>((new Mock<ILogger<UserManager<ReheeCmfBaseUser>>>()).Object);
        var tokenProvider = typeof(ReheeCmfDefaultAuthenticationProvider<ReheeCmfBaseUser>);

        services.AddIdentity<ReheeCmfBaseUser, TenantIdentityRole>(
          options => options.SignIn.RequireConfirmedAccount = false)
          .AddEntityFrameworkStores<CmfIdentityContext<ReheeCmfBaseUser>>()
          .AddTokenProvider("Default", tokenProvider);

        services.AddScoped<RoleManager<TenantIdentityRole>>();

        services.AddScoped<UserManager<ReheeCmfBaseUser>>();
        services.AddScoped<SignInManager<ReheeCmfBaseUser>, TenantSignInManager<ReheeCmfBaseUser>>();

        if (typeof(ReheeCmfBaseUser) != typeof(IdentityUser))
        {
          services.AddDefaultUser<ReheeCmfBaseUser, CmfIdentityContext<ReheeCmfBaseUser>>(sp =>
            sp.GetService<CmfIdentityContext<ReheeCmfBaseUser>>()!);
        }
        services.AddSingleton<UserManagementOption>(sp => UserManagementOption.Detault);
        services.AddScoped<IUserService, UserService<ReheeCmfBaseUser, TenantIdentityRole, TenantIdentityUserRole>>();

      });
    }

    [Test]
    public async Task GetUserServiceTes()
    {
      var service = sp.GetService<IUserService>();
      var passwordString = "Change_me_123!";
      var id = await service!.CreateUserAsync(
        new Dictionary<string, string?>
        {
          ["UserName"] = "username",
          ["Email"] = "Email1@email.com",
          ["Password"] = passwordString,
          ["EmailConfirmed"] = "true",
        }, null);

      var usermanager = sp.GetService<UserManager<ReheeCmfBaseUser>>();
      var user = await usermanager.FindByIdAsync(id);
      var password = await usermanager.CheckPasswordAsync(user, passwordString);
      Assert.True(password);
      Assert.False(user.EmailConfirmed);
      await service!.UpdateUserAsync(id,
        new Dictionary<string, string?>()
        {
          ["UserName"] = "username_1",
          ["Email"] = "Email1@email.com_1",
          ["Password"] = passwordString,
          ["EmailConfirmed"] = "true",
        });
      var user2 = await usermanager.FindByIdAsync(id);
      var password2 = await usermanager.CheckPasswordAsync(user2, passwordString);
      Assert.True(password);
      Assert.True(user.EmailConfirmed);
    }
    [Test]
    public async Task GetUserService_ResetPassword()
    {
      var service = sp.GetService<IUserService>();
      var passwordString = "Change_me_123!";
      var id = await service!.CreateUserAsync(
        new Dictionary<string, string?>
        {
          ["UserName"] = "username",
          ["Email"] = "Email1@email.com",
          ["Password"] = passwordString,
          ["EmailConfirmed"] = "true",
        }, null);

      var usermanager = sp.GetService<UserManager<ReheeCmfBaseUser>>();
      var user = await usermanager.FindByIdAsync(id);
      var password = await usermanager.CheckPasswordAsync(user, passwordString);
      Assert.True(password);
      Assert.False(user.EmailConfirmed);

      await service!.ResetPassword(id, $"{passwordString}__1", null);
      var user2 = await usermanager.FindByIdAsync(id);
      var password2 = await usermanager.CheckPasswordAsync(user2, $"{passwordString}__1");
      Assert.True(password2);
      await service!.ResetPassword(id, null, null);
      var user3 = await usermanager.FindByIdAsync(id);
      var password3 = await usermanager.CheckPasswordAsync(user3, $"{user3.UserName}_Password1");
      Assert.True(password3);
    }
  }
}
