using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineMarketplace.Data;
using OnlineMarketplace.Entities;

namespace OnlineMarketplace.Areas.Profile.Pages.Wishlist
{
    public class IndexModel : PageModel
    {
        private readonly OnlineMarketplace.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(OnlineMarketplace.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public WishList WishList { get;set; } = default!;

        public ApplicationUser user { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        public IEnumerable<Entities.File> Files { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.WishList != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var users = _context.Users.ToList();
                    var reviews = _context.Review.ToList();
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    var wishlists = _context.WishList.Include(c => c.Products).ToList();
                    var wishlist = wishlists.FirstOrDefault(w => w.UserID == user.Id);
                    var files = _context.File.ToList();
                    WishList = wishlist;
                    Users = users;
                    Reviews = reviews;
                    Files = files;
                    this.user = user;
                }
            }
        }

        public async Task<IActionResult> OnPost(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var wishlists = _context.WishList.Include(c => c.Products).ToList();
            var wishlist = wishlists.FirstOrDefault(w => w.UserID == user.Id);
            var product = _context.Product.FirstOrDefault(p => p.ProductID == id);
            if (wishlist.Products.Exists(p => p.ProductID == id))
            {
                wishlist.Products.Remove(product);
            }
            product.NumberOfSales = product.NumberOfSales + 1;
            var sale = new Sale { ProductID = id, UserID = user.Id, SaleDate = DateTime.Now };
            _context.Sale.Add(sale);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
