using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ReheeCmf.Authenticates;
using ReheeCmf.AuthenticationModule.Services;
using ReheeCmf.Commons;
using ReheeCmf.Entities;
using ReheeCmf.Services;
using System.Text;

namespace ReheeCmf.AuthenticationModule
{
  public static class CmfAuthenticationModuleSetup
  {
    public static IServiceCollection AddCmfAuthentication<TUser>(
      this IServiceCollection services, IConfiguration configuration, CrudOption? optionCrud = null)
    where TUser : IdentityUser, ICmfUser, new()
    {
      var tokenConfig = configuration.GetOption<TokenManagement>();
      services.AddSingleton<TokenManagement>(tokenConfig);
      if (optionCrud == null)
      {
        optionCrud = configuration.GetOption<CrudOption>();
        services.AddSingleton<CrudOption>(optionCrud);
      }
      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Secret ?? "123456789012345678")),
        ValidIssuer = tokenConfig.Issuer,
        ValidAudience = tokenConfig.Audience,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
      };
      services.AddSingleton<TokenValidationParameters>(sp => tokenValidationParameters);
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(x =>
        {
          x.RequireHttpsMetadata = false;
          x.SaveToken = true;
          x.TokenValidationParameters = tokenValidationParameters;
        });
      var s = services.Where(b => b.ServiceType == typeof(IServiceProvider)).ToList();

      services.AddScoped<IUserTokenStorage, UserTokenStorage>();
      services.AddScoped<IUserLockendStorage, UserLockendStorage>();
      services.AddScoped<IJWTService, JWTService<TUser>>();
      //  (sp =>
      //{
      //  return new JWTService<TUser>(
      //    sp,
      //    sp.GetService<UserManager<TUser>>(),
      //    sp.GetService<SignInManager<TUser>>(),
      //    tokenConfig,
      //    sp.GetService<IHttpContextAccessor>(),
      //    optionCrud,
      //    sp.GetService<IUserTokenStorage>(),
      //    sp.GetService<IUserLockendStorage>());
      //});
      services.AddScoped<IJWTService<TUser>>(sp =>
      {
        var service = sp.GetService<IJWTService>();
        if (service is IJWTService<TUser> a)
        {
          return a;
        }
        return default!;
      });
      return services;
    }
  }
}
