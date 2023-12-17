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

namespace OnlineMarketplace.Areas.Profile.Pages.Downloads
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

        /*public IList<Sale> Sale { get;set; } = default!;*/

        public IEnumerable<Sale> Sales { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        public ApplicationUser user { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Sale != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    var sales = _context.Sale.Where(r => r.UserID == user.Id).ToList();
                    var products = _context.Product.ToList();
                    var reviews = _context.Review.ToList();
                    Sales = sales;
                    Products = products;
                    this.user = user;
                    Reviews = reviews;
                }
            }
        }
    }
}
