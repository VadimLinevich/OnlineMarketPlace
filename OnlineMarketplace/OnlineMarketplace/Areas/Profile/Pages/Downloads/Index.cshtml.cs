using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMarketplace.Data;
using OnlineMarketplace.Entities;

namespace OnlineMarketplace.Areas.Profile.Pages.Downloads
{
    public class IndexModel : PageModel
    {
        private readonly OnlineMarketplace.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;

        public IndexModel(OnlineMarketplace.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

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

        public async Task<IActionResult> OnPost(int id)
        {
            var DBFiles = _context.File.Where(f => f.ProductID == id).ToList();
            string file_path = Path.Combine(_appEnvironment.WebRootPath, "Files/" + "DownloadFile" + Path.GetExtension(DBFiles[0].FileTitle));
            using (FileStream fs = System.IO.File.Create(file_path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(DBFiles[0].Content);
                fs.Write(info, 0, info.Length);
            }
            string[] files = Directory.GetFiles(_appEnvironment.WebRootPath + "/Files", "DownloadFile" + ".*");

            FileInfo file = new FileInfo(file_path);
            if (file.Exists)
            {
                string file_type = "text/plain";
                string file_name = DBFiles[0].FileTitle;
                var result = PhysicalFile(file_path, file_type, file_name);

                Response.OnCompleted(async () =>
                {
                    if (files.Length > 0)
                    {
                        await Task.Run(() => System.IO.File.Delete(files[0]));
                    }
                });
                return result;
            }
            return RedirectToPage("./Index");
        }
    }
}
