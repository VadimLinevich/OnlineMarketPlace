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

namespace OnlineMarketplace.Areas.Profile.Pages.Reviews
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

        /*public IList<Review> Review { get;set; } = default!;*/

        public IEnumerable<Review> Reviews { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public ApplicationUser user { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Review != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    /*var product = await _context.Product.FindAsync(user.Id);*/
                    var reviews = _context.Review.Where(r => r.UserID == user.Id).ToList();
                    var products = _context.Product.ToList();
                    Reviews = reviews;
                    Products = products;
                    this.user = user;
                    /*Review = await _context.Review
                    .Include(r => r.Product)
                    .Include(r => r.User).ToListAsync();*/
                }
            }
        }
    }
}
