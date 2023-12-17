﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMarketplace.Data;
using OnlineMarketplace.Entities;

namespace OnlineMarketplace.Areas.Profile.Pages.Settings
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
        public ApplicationUser user { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                this.user = user;
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile uploadedFile)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ModelState.Remove("user.WishList");
            ModelState.Remove("uploadedfile");
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Something get wrong");
                this.user = user;
                return Page();
            }
            if (_context.Users.FirstOrDefault(u => u.UserName == this.user.UserName && u.Email == this.user.Email && u.Name == this.user.Name && u.LastName == this.user.LastName) != null && uploadedFile == null)
            {
                ModelState.AddModelError(string.Empty, "Change some fields before updating");
                this.user = user;
                return Page();
            }
            if (_context.Users.FirstOrDefault(u => (u.UserName == this.user.UserName || u.Email == this.user.Email) && (u.UserName != user.UserName || u.Email != user.Email)) != null)
            {
                ModelState.AddModelError(string.Empty, "Such email or username already exists");
                this.user = user;
                return Page();
            }

            if (uploadedFile != null)
            {
                if(System.IO.File.Exists(_appEnvironment.WebRootPath + user.Avatar.Substring(1)))
                {
                    System.IO.File.Delete(_appEnvironment.WebRootPath + user.Avatar.Substring(1));
                }
                string path = "/Avatars/" + user.Id + "-" + uploadedFile.FileName;
                if (Path.GetExtension(path) != ".jpg" && Path.GetExtension(path) != ".png")
                {
                    ModelState.AddModelError(string.Empty, "File type should have jpg or png extension");
                    this.user = user;
                    return Page();
                }
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                user.Avatar = "~" + path;
            }
            user.Name = this.user.Name;
            user.LastName = this.user.LastName;
            user.Email = this.user.Email;
            user.NormalizedEmail = this.user.Email.ToUpper();
            user.UserName = this.user.UserName;
            user.NormalizedUserName = this.user.UserName.ToUpper();

            await _context.SaveChangesAsync();

            return RedirectToPage("./Edit");
        }
    }
}
