using CmfDemo.Models;
using Microsoft.AspNetCore.Mvc;
using ReheeCmf.Requests;
using System.Diagnostics;

namespace CmfDemo.Controllers
{
  public class HomeController : Controller
  {
    private readonly IHttpClientFactory fac;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IHttpClientFactory fac, ILogger<HomeController> logger)
    {
      this.fac = fac;
      _logger = logger;
    }

    public IActionResult Index()
    {
      var c1 = fac.CreateClient("1");
      var c2 = fac.CreateClient("1");
      return Ok(c1.GetHashCode() == c2.GetHashCode());
    }

  }
}
