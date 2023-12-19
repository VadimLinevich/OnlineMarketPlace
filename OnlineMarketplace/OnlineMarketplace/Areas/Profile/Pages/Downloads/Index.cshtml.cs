using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using OnlineMarketplace.Data;
using OnlineMarketplace.Entities;
using static System.Net.WebRequestMethods;

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

        public IEnumerable<Entities.File> Files { get; set; }

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
                    var files = _context.File.ToList();
                    Sales = sales;
                    Products = products;
                    this.user = user;
                    Reviews = reviews;
                    Files = files;
                }
            }
        }

        public async Task<IActionResult> OnPost(int id)
        {
            var DBFiles = _context.File.Where(f => f.ProductID == id).ToList();
            foreach(var DBFile in DBFiles)
            {
                string file_path = Path.Combine(_appEnvironment.WebRootPath, "Files/" + DBFile.FileTitle);
                using (FileStream fs = System.IO.File.Create(file_path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(DBFile.Content);
                    fs.Write(info, 0, info.Length);
                }
            }

            string fpath = Path.Combine(_appEnvironment.WebRootPath, "Downloads/Download.zip");
            ZipFile.CreateFromDirectory(Path.Combine(_appEnvironment.WebRootPath, "Files"), fpath);
            string[] files = Directory.GetFiles(_appEnvironment.WebRootPath + "/Files", "*.*")
                             .Where(file => Path.GetExtension(file).ToLower() != ".zip")
                             .ToArray();
            foreach (string path in files)
            {
                System.IO.File.Delete(path);
            }
            FileInfo file = new FileInfo(fpath);
            if (file.Exists)
            {
                string file_type = "application/zip";
                string file_name = "Download.zip";
                var result = PhysicalFile(fpath, file_type, file_name);

                Response.OnCompleted(async () =>
                {
                    await Task.Run(() => System.IO.File.Delete(fpath));
                });
                return result;
            }
            return RedirectToPage("./Index");
        }
    }
}
