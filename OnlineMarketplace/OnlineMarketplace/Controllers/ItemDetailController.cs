using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarketplace.Data;
using OnlineMarketplace.Entities;
using OnlineMarketplace.View_Models;

namespace OnlineMarketplace.Controllers
{
    public class ItemDetailController : Controller
    {
        private ApplicationDbContext dbcontext;
        private UserManager<ApplicationUser> usermanager;


        public ItemDetailController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            dbcontext = dbContext;
            usermanager = userManager;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if(id != null)
            {
                var product = await dbcontext.Product.FindAsync(id);
                if(product != null)
                {
                    var SignInuser = await usermanager.GetUserAsync(HttpContext.User);
                    List<WishList> wishlists = new List<WishList>();
                    if (SignInuser != null)
                    {
                        wishlists = dbcontext.WishList.Include(c => c.Products).Where(w => w.UserID == SignInuser.Id).ToList();
                    }
                    var user = await dbcontext.Users.FindAsync(product.UserID);
                    string filesTypes = "";
                    var files = dbcontext.File.Where(f => f.ProductID == id).ToList();
                    var sales = dbcontext.Sale.ToList();
                    foreach (var file in files)
                    {
                        if (!filesTypes.Contains(System.IO.Path.GetExtension(file.FileTitle)))
                        {
                            filesTypes = filesTypes + System.IO.Path.GetExtension(file.FileTitle) + "," + " ";
                        }
                    }
                    filesTypes = filesTypes.Trim();
                    filesTypes = filesTypes.Remove(filesTypes.Length - 1);
                    ViewData["FileTypes"] = filesTypes.ToUpper().Replace(".", "");
                    ItemDetailViewModel itemDetailViewModel = new ItemDetailViewModel { user = user, product = product, WishLists = wishlists, Sales = sales};
                    return View(itemDetailViewModel);
                }
            }
            return RedirectToAction("Index", "Item");
        }

        [Authorize]
        [HttpPost]
        [Route("ItemDetail/Index/{id:int}")]
        public async Task<IActionResult> AddToWishlist(int? id)
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
                return RedirectToAction("Index", new {id});
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = $"/ItemDetail/Index/{id}" });
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
                var sale = new Sale { ProductID = id ?? (0), UserID = user.Id, SaleDate = DateTime.Now };
                dbcontext.Sale.Add(sale);
                await dbcontext.SaveChangesAsync();
                return RedirectToAction("Index", new {id});
            }
            return RedirectToAction("Index");
        }
    }
}
