using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMarketplace.Data;
using OnlineMarketplace.Entities;

namespace OnlineMarketplace.Areas.Profile.Pages.Dashboard
{
    public class EditModel : PageModel
    {
        private readonly OnlineMarketplace.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;

        public EditModel(OnlineMarketplace.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

        [BindProperty]
        public Product Product { get; set; }

        public ApplicationUser user { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return RedirectToPage("./Index");
            }

            var product =  await _context.Product.FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return RedirectToPage("./Index");
            }
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                this.user = user;
                if (product.UserID != user.Id)
                {
                    return RedirectToPage("./Index");
                }
                ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
                Product = product;
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile uploadedFile)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var product = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == Product.ProductID);
            ModelState.Remove("uploadedfile");
            ModelState.Remove("Product.User");
            ModelState.Remove("Product.UserID");
            ModelState.Remove("Product.ProductImage");
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Something get wrong");
                this.user = user;
                Product = product;
                ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
                return Page();
            }
            if (uploadedFile != null)
            {
                if (System.IO.File.Exists(_appEnvironment.WebRootPath + product.ProductImage.Substring(1)))
                {
                    System.IO.File.Delete(_appEnvironment.WebRootPath + product.ProductImage.Substring(1));
                }
                string path = "/TitleImages/" + Guid.NewGuid() + "-" + uploadedFile.FileName;
                if (Path.GetExtension(path) != ".jpg" && Path.GetExtension(path) != ".png")
                {
                    ModelState.AddModelError(string.Empty, "File type should have jpg or png extension");
                    this.user = user;
                    ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
                    return Page();
                }
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                product.ProductImage = "~" + path;
            }

            product.CategoryID = Product.CategoryID;
            product.ProductTitle = Product.ProductTitle;
            product.Description = Product.Description;
            product.SoftwareVersion = Product.SoftwareVersion;
            product.Price = Product.Price;
            product.Is_Deleted = Product.Is_Deleted;

            await _context.SaveChangesAsync();

            /*_context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }*/

            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }
    }
}
