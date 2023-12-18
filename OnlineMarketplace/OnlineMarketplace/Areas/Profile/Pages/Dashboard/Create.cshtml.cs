using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineMarketplace.Data;
using OnlineMarketplace.Entities;

namespace OnlineMarketplace.Areas.Profile.Pages.Dashboard
{
    public class CreateModel : PageModel
    {
        private readonly OnlineMarketplace.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;

        public CreateModel(OnlineMarketplace.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                this.user = user;

                ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
            }
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } 

        public ApplicationUser user { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile uploadedFile, IFormFile projectFile)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ModelState.Remove("projectfile");
            ModelState.Remove("uploadedfile");
            ModelState.Remove("Product.User");
            ModelState.Remove("Product.UserID");
            ModelState.Remove("Product.ProductImage");
            if (!ModelState.IsValid || _context.Product == null || Product == null)
            {
                ModelState.AddModelError(string.Empty, "Something get wrong");
                this.user = user;
                ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
                return Page();
            }
            if(uploadedFile == null)
            {
                ModelState.AddModelError(string.Empty, "Choose product image");
                this.user = user;
                ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
                return Page();
            }
            if (projectFile == null)
            {
                ModelState.AddModelError(string.Empty, "Choose product file");
                this.user = user;
                ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
                return Page();
            }

            if (projectFile != null)
            {
                var result = new StringBuilder();
                using (var stream = projectFile.OpenReadStream())
                {
                    byte[] buffer = new byte[512];

                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {

                        for (int i = 0; i < bytesRead; i++)
                        {
                            if (buffer[i] < 32 && buffer[i] != 9 && buffer[i] != 10 && buffer[i] != 13)
                            {
                                ModelState.AddModelError(string.Empty, "Invalid product file");
                                this.user = user;
                                ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
                                return Page();
                            }
                        }

                    }
                }
                using (var reader = new StreamReader(projectFile.OpenReadStream()))
                {
                    while (reader.Peek() >= 0) 
                    { 
                        result.AppendLine(await reader.ReadLineAsync());
                    }
                }
                string fileContent = result.ToString();
                Entities.File file = new Entities.File { Product = Product, FileTitle = projectFile.FileName, Content = fileContent };
                _context.File.Add(file);
            }

            if (uploadedFile != null)
            {
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
                Product.ProductImage = "~" + path;
            }
            Product.UserID = user.Id;
            Product.NumberOfSales = 0;
            Product.CreateDate = DateTime.Now;
            Product.Is_Deleted = false;
            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
