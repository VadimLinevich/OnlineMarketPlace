using Microsoft.AspNetCore.Mvc;
using OnlineMarketplace.Models;
using System.Diagnostics;

namespace OnlineMarketplace.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}