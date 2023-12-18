using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OnlineMarketplace.Data;
using OnlineMarketplace.Entities;
using OnlineMarketplace.View_Models;

namespace OnlineMarketplace.Controllers
{
    public class ReviewController : Controller
    {
        private ApplicationDbContext dbcontext;
        private UserManager<ApplicationUser> usermanager;


        public ReviewController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            dbcontext = dbContext;
            usermanager = userManager;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
            {
                var product = await dbcontext.Product.FindAsync(id);
                if (product != null)
                {
                    var SignInuser = await usermanager.GetUserAsync(HttpContext.User);
                    List<WishList> wishlists = new List<WishList>();
                    if (SignInuser != null)
                    {
                        wishlists = dbcontext.WishList.Include(c => c.Products).Where(w => w.UserID == SignInuser.Id).ToList();
                    }
                    var user = await dbcontext.Users.FindAsync(product.UserID);
                    var reviews = dbcontext.Review.Where(r => r.ProductID == id).ToList();
                    var users = dbcontext.Users.ToList();
                    string filesTypes = "";
                    var files = dbcontext.File.Where(f => f.ProductID == id).ToList();
                    foreach (var file in files)
                    {
                        filesTypes = Path.GetExtension(file.FileTitle) + "," + " ";
                    }
                    filesTypes = filesTypes.Trim();
                    filesTypes = filesTypes.Remove(filesTypes.Length - 1);
                    ViewData["FileTypes"] = filesTypes.ToUpper().Replace(".", "");
                    ItemDetailViewModel itemDetailViewModel = new ItemDetailViewModel { user = user, product = product, WishLists = wishlists, Reviews = reviews, Users = users };
                    if (SignInuser != null)
                    {
                        var userReview = itemDetailViewModel.Reviews.FirstOrDefault(r => r.UserID == SignInuser.Id);
                        if (userReview != null)
                        {
                            itemDetailViewModel.review = userReview;
                        }
                    }
                    return View(itemDetailViewModel);
                }
            }
            return RedirectToAction("Index", "Item");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Review review, int id)
        {
            ModelState.Remove("review.UserID");
            ModelState.Remove("review.User");
            ModelState.Remove("review.Product");
            if (ModelState.IsValid)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = $"/Review/Index/{id}" });
                }
                var user = await usermanager.GetUserAsync(HttpContext.User);
                review.UserID = user.Id;
                review.ProductID = id;
                review.WriteTime = DateTime.Now;
                dbcontext.Review.Add(review);
                await dbcontext.SaveChangesAsync();

                return RedirectToAction("Index", new { id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Something get wrong");
                var product = await dbcontext.Product.FindAsync(id);
                if (product != null)
                {
                    var SignInuser = await usermanager.GetUserAsync(HttpContext.User);
                    List<WishList> wishlists = new List<WishList>();
                    if (SignInuser != null)
                    {
                        wishlists = dbcontext.WishList.Include(c => c.Products).Where(w => w.UserID == SignInuser.Id).ToList();
                    }
                    var user = await dbcontext.Users.FindAsync(product.UserID);
                    var reviews = dbcontext.Review.Where(r => r.ProductID == id).ToList();
                    var users = dbcontext.Users.ToList();
                    string filesTypes = "";
                    var files = dbcontext.File.Where(f => f.ProductID == id).ToList();
                    foreach (var file in files)
                    {
                        filesTypes = Path.GetExtension(file.FileTitle) + "," + " ";
                    }
                    filesTypes = filesTypes.Trim();
                    filesTypes = filesTypes.Remove(filesTypes.Length - 1);
                    ViewData["FileTypes"] = filesTypes.ToUpper().Replace(".", "");
                    ItemDetailViewModel itemDetailViewModel = new ItemDetailViewModel { user = user, product = product, WishLists = wishlists, Reviews = reviews, Users = users };
                    if (SignInuser != null)
                    {
                        var userReview = itemDetailViewModel.Reviews.FirstOrDefault(r => r.UserID == SignInuser.Id);
                        if (userReview != null)
                        {
                            itemDetailViewModel.review = userReview;
                        }
                    }
                    return View("Index", itemDetailViewModel);
                }
                return RedirectToAction("Index", "Item");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Review review, int id)
        {
            ModelState.Remove("review.UserID");
            ModelState.Remove("review.User");
            ModelState.Remove("review.Product");
            if (ModelState.IsValid)
            {
                var user = await usermanager.GetUserAsync(HttpContext.User);
                var NewReview = await dbcontext.Review.FindAsync(user.Id, id);
                NewReview.Grade = review.Grade;
                NewReview.Content = review.Content;
                NewReview.WriteTime = DateTime.Now;
                await dbcontext.SaveChangesAsync();

                return RedirectToAction("Index", new { id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Something get wrong");
                var product = await dbcontext.Product.FindAsync(id);
                if (product != null)
                {
                    var SignInuser = await usermanager.GetUserAsync(HttpContext.User);
                    List<WishList> wishlists = new List<WishList>();
                    if (SignInuser != null)
                    {
                        wishlists = dbcontext.WishList.Include(c => c.Products).Where(w => w.UserID == SignInuser.Id).ToList();
                    }
                    var user = await dbcontext.Users.FindAsync(product.UserID);
                    var reviews = dbcontext.Review.Where(r => r.ProductID == id).ToList();
                    var users = dbcontext.Users.ToList();
                    string filesTypes = "";
                    var files = dbcontext.File.Where(f => f.ProductID == id).ToList();
                    foreach (var file in files)
                    {
                        filesTypes = Path.GetExtension(file.FileTitle) + "," + " ";
                    }
                    filesTypes = filesTypes.Trim();
                    filesTypes = filesTypes.Remove(filesTypes.Length - 1);
                    ViewData["FileTypes"] = filesTypes.ToUpper().Replace(".", "");
                    ItemDetailViewModel itemDetailViewModel = new ItemDetailViewModel { user = user, product = product, WishLists = wishlists, Reviews = reviews, Users = users };
                    if (SignInuser != null)
                    {
                        var userReview = itemDetailViewModel.Reviews.FirstOrDefault(r => r.UserID == SignInuser.Id);
                        if (userReview != null)
                        {
                            itemDetailViewModel.review = userReview;
                        }
                    }
                    return View("Index", itemDetailViewModel);
                }
                return RedirectToAction("Index", "Item");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Review/Index/{id:int}")]
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
                return RedirectToAction("Index", new { id });
            }
            return RedirectToAction("Index");
        }
    }
}
