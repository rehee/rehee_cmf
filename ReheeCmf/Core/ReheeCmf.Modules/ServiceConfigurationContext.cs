using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Modules
{
  public class ServiceConfigurationContext
  {
    public IServiceCollection? Services { get; set; }
    public IMvcBuilder? MvcBuilder { get; set; }
    public IConfiguration? Configuration { get; set; }
    public CrudOption? CrudOptions { get; set; }
    public Action<object>[]? BuilderEntityOData { get; set; }
    public Action<object>[]? BuilderUserOData { get; set; }
    public (string, Action<object>)[]? AdditionalOData { get; set; }
    public ConfigureWebHostBuilder? WebHost { get; set; }
    public WebApplication? App { get; set; }
    public IWebHostEnvironment? Env { get; set; }
    public TenantSetting? Tenant { get; set; }
  }
}
