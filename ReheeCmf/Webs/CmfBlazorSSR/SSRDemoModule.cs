using CmfBlazorSSR.Components;
using CmfBlazorSSR.Components.Account;
using CmfBlazorSSR.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using ReheeCmf;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Contexts;
using ReheeCmf.Modules;

namespace CmfBlazorSSR
{
  public class SSRDemoModule : CmfApiModule<ApplicationDbContext, ApplicationUser>
  {
    public override string ModuleTitle => "SSRDemoModule";

    public override string ModuleName => "SSRDemoModule";

    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      return Task.FromResult(Enumerable.Empty<string>());
    }
    public override async Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.PreConfigureServicesAsync(context);
      context.UseAuthenticationAfterRouting = true;
    }
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.ConfigureServicesAsync(context);
      context.Services!.AddRazorComponents()
        .AddInteractiveServerComponents();
      
      context.Services!.AddCascadingAuthenticationState();
      context.Services!.AddScoped<IdentityUserAccessor>();
      context.Services!.AddScoped<IdentityRedirectManager>();
      context.Services!.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();


      context.Services!.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
      
    }
    public override async Task BeforePreApplicationInitializationAsync(ServiceConfigurationContext context)
    {
      await base.BeforePreApplicationInitializationAsync(context);
      context.App!.UseHttpsRedirection();
    }
    public override async Task ApplicationInitializationAsync(ServiceConfigurationContext context)
    {
      await base.ApplicationInitializationAsync(context);
      var app = context.App;
      var env = context.Env;

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
        //app.UseReverseProxyHttpsEnforcer();
      }

      context.App!.UseAntiforgery();
      context.App!.MapRazorComponents<App>()
          .AddInteractiveServerRenderMode();

      // Add additional endpoints required by the Identity /Account Razor components.
      context.App!.MapAdditionalIdentityEndpoints();

    }
  }
}
