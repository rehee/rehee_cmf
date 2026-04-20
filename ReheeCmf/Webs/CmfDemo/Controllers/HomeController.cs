using CmfDemo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Authenticates;
using ReheeCmf.Contexts;
using ReheeCmf.Requests;

namespace CmfDemo.Controllers
{
	public class HomeController : Controller
	{
		private readonly IRequestClient<IAuthorize> request;
		private readonly ApplicationDbContext db;
		private readonly IContext context;
		private readonly IServiceProvider sp;

		public HomeController(IRequestClient<IAuthorize> request, ApplicationDbContext db, IContext context, IServiceProvider sp)
		{
			this.request = request;
			this.db = db;
			this.context = context;
			this.sp = sp;
		}

		public async Task<IActionResult> Index()
		{
			var e1 = await context.Query<Entity1>(true).FirstOrDefaultAsync();
			return Ok();
		}

		public async Task<IActionResult> Index2()
		{
			var result = await request.Request<Entity1>(HttpMethod.Get, "https://localhost:5001/Test", name: "123", ignoreToken: true);
			return Ok();
		}
	}
}
