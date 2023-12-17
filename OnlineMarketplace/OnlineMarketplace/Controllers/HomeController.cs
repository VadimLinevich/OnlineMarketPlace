using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarketplace.Data;
using OnlineMarketplace.Entities;
using OnlineMarketplace.Models;
using System.Diagnostics;

namespace OnlineMarketplace.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext dbcontext;
        private UserManager<ApplicationUser> usermanager;


        public HomeController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            dbcontext = dbContext;
            usermanager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BecomeSeller()
        {
            var user = await usermanager.GetUserAsync(HttpContext.User);
            await usermanager.RemoveFromRoleAsync(user, "user");
            await usermanager.AddToRoleAsync(user, "seller");
            await dbcontext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}