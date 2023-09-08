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

    public IActionResult Index()
    {

      var entityList = db.Entity1s.ToList();
      return Ok();
    }

  }
}
