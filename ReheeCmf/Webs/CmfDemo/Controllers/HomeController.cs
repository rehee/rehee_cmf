using CmfDemo.Data;
using CmfDemo.Models;
using Microsoft.AspNetCore.Mvc;
using ReheeCmf.Contexts;
using ReheeCmf.Requests;
using System.Diagnostics;

namespace CmfDemo.Controllers
{
  public class HomeController : Controller
  {
    private readonly ApplicationDbContext db;
    private readonly IContext context;

    public HomeController(ApplicationDbContext db, IContext context)
    {
      this.db = db;
      this.context = context;
    }

    public async Task<IActionResult> Index()
    {
      var e1 = context.Query<Entity1>(true).ToList();
      e1.Reverse();
      var newE = new Entity1();
      db.Add(newE);
      ////await context.AddAsync<Entity1>(newE, CancellationToken.None);
      //newE.Name2 = Guid.NewGuid().ToString();
      //context.Delete(typeof(Entity1), e1.LastOrDefault().Id);
      await context.SaveChangesAsync(null);
      //await context.SaveChangesAsync(null);
      //db.SaveChanges();
      return Ok();
    }

  }
}
