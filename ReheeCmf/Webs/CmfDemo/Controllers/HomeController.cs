using CmfDemo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Contexts;

namespace CmfDemo.Controllers
{
  public class HomeController : Controller
  {
    private readonly ApplicationDbContext db;
    private readonly IContext context;
    private readonly IServiceProvider sp;

    public HomeController(ApplicationDbContext db, IContext context, IServiceProvider sp)
    {
      this.db = db;
      this.context = context;
      this.sp = sp;
    }

    public async Task<IActionResult> Index()
    {
      var e1 = await context.Query<Entity1>(true).FirstOrDefaultAsync();
      return Ok();
    }

  }
}
