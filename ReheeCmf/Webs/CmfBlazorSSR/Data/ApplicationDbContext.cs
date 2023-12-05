using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.ContextModule.Contexts;

namespace CmfBlazorSSR.Data
{
  public class ApplicationDbContext : CmfIdentityContext<ApplicationUser>
  {
    public ApplicationDbContext(IServiceProvider sp) : base(sp)
    {
    }
  }
}
