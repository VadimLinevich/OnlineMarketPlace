using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarketplace.Data;
using OnlineMarketplace.Entities;
using OnlineMarketplace.View_Models;

namespace OnlineMarketplace.Controllers
{
    public class ItemController : Controller
    {

        private ApplicationDbContext dbcontext;
        private UserManager<ApplicationUser> usermanager;


        public ItemController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            dbcontext = dbContext;
            usermanager = userManager;
        }
        public async Task<IActionResult> Index(string sort = "Newest", string category = "All", string q = "")
        {
            var user = await usermanager.GetUserAsync(HttpContext.User);
            List<WishList> wishlists = new List<WishList>();
            if (user != null)
            {
                wishlists = dbcontext.WishList.Include(c => c.Products).Where(w => w.UserID == user.Id).ToList();
            }
            var users = dbcontext.Users.ToList();
            var products = dbcontext.Product.Where(p => p.Is_Deleted == false).ToList();
            var reviews = dbcontext.Review.ToList();
            if (!String.IsNullOrEmpty(q))
            {
                products = products.Where(s => s.ProductTitle.Contains(q) || s.ProductTitle.Contains(q.ToUpper())).ToList();
                category = "All";
                sort = "Newest";
            }
            if (category != "All")
            {
                products = dbcontext.Product.Where(p => p.Category!.CategoryName == category && p.Is_Deleted == false).ToList();
            }
            if (sort == "Newest")
            {
                products = products.OrderByDescending(p => p.CreateDate).ToList();
            }
            if (sort == "Bestsellers")
            {
                products = products.OrderByDescending(p => p.NumberOfSales).ToList();
            }
            if (sort == "Best rated")
            {
                products = products.OrderByDescending(p => p.NumberOfSales).ToList();
            }
            if (sort == "Price")
            {
                products = products.OrderByDescending(p => p.Price).ToList();
            }
            ViewData["Sort"] = sort;
            ViewData["Category"] = category;
            ViewData["SearchString"] = q;
            ItemsViewModel itemsViewModel = new ItemsViewModel { Users = users, Products = products, Reviews = reviews, WishLists = wishlists };
            return View(itemsViewModel);
        }

        [Authorize]
        [HttpPost]
        [Route("Item")]
        public async Task<IActionResult> AddToWishlist(int? id, string sort = "Newest", string category = "All", string q = "")
        {
            if (id != null)
            {
                var user = await usermanager.GetUserAsync(HttpContext.User);
                var wishlists = dbcontext.WishList.Include(c => c.Products).ToList();
                var wishlist = wishlists.FirstOrDefault(w => w.UserID == user.Id);
                var product = dbcontext.Product.FirstOrDefault(p => p.ProductID == id);
                if (wishlist.Products.Exists(p => p.ProductID == id))
                {
                    wishlist.Products.Remove(product);
                    dbcontext.SaveChanges();
                }
                else
                {
                    wishlist.Products.Add(product);
                    dbcontext.SaveChanges();
                }
                return RedirectToAction("Index", new {sort = sort, category = category, q = q });
            }
            return RedirectToAction("Index");
        }
    }
}
