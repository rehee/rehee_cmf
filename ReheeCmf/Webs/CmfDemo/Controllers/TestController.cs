using CmfDemo.Data;
using Microsoft.AspNetCore.Mvc;
using ReheeCmf.Contexts;

namespace CmfDemo.Controllers
{
	[ApiController]
	[Route("Test")]
	public class TestController : Controller
	{
		private readonly IContext db;

		public TestController(IContext db)
		{
			this.db = db;
		}
		public async Task<IActionResult> Index()
		{
			await db.AddAsync<Entity1>(new Entity1 { });
			db.SaveChanges();
			return View();
		}
	}
}
