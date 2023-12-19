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
            var files = dbcontext.File.ToList();
            var sales = dbcontext.Sale.ToList();
            if (!String.IsNullOrEmpty(q))
            {
                products = products.Where(s => s.ProductTitle.Contains(q) || s.ProductTitle.ToUpper().Contains(q.ToUpper())).ToList();
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
                products = dbcontext.Product.Include(p => p.Reviews).Where(p => p.Reviews.Count != 0).ToList();
                var ProductWithZeroReviews = dbcontext.Product.Where(p => p.Reviews.Count == 0).ToList();
                products = products.OrderByDescending(p => p.Reviews.Average(r => r.Grade)).ToList();
                products.AddRange(ProductWithZeroReviews);
            }
            if (sort == "Price")
            {
                products = products.OrderByDescending(p => p.Price).ToList();
            }
            ViewData["Sort"] = sort;
            ViewData["Category"] = category;
            ViewData["SearchString"] = q;
            ItemsViewModel itemsViewModel = new ItemsViewModel { Users = users, Products = products, Reviews = reviews, WishLists = wishlists, Files = files, Sales = sales };
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

        [HttpPost]
        public async Task<IActionResult> Buy(int? id, string sort = "Newest", string category = "All", string q = "")
        {
            if (!User.Identity.IsAuthenticated)
            {
                if(q == "")
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = $"/Item?sort={sort}&category={category}" });
                }
                else
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = $"/Item?q={q}" });
                }
                
            }
            if (id != null)
            {
                var user = await usermanager.GetUserAsync(HttpContext.User);
                var wishlists = dbcontext.WishList.Include(c => c.Products).ToList();
                var wishlist = wishlists.FirstOrDefault(w => w.UserID == user.Id);
                var product = dbcontext.Product.FirstOrDefault(p => p.ProductID == id);
                if (wishlist.Products.Exists(p => p.ProductID == id))
                {
                    wishlist.Products.Remove(product);
                }
                product.NumberOfSales = product.NumberOfSales + 1;
                var sale = new Sale { ProductID = id??(0), UserID = user.Id, SaleDate = DateTime.Now };
                dbcontext.Sale.Add(sale);
                await dbcontext.SaveChangesAsync();
                return RedirectToAction("Index", new { sort = sort, category = category, q = q });
            }
            return RedirectToAction("Index");
        }
    }
}
