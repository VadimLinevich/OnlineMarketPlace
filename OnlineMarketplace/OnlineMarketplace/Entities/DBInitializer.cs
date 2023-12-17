using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using OnlineMarketplace.Data;

namespace OnlineMarketplace.Entities
{
    public class DBInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string password = "123456789";
            string userEmail = "user@gmail.com";
            string sellerEmail = "seller@gmail.com";
            if (await roleManager.FindByNameAsync("seller") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("seller"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                ApplicationUser user = new() { Email = userEmail, UserName = "MSTdev", Name = "Vadim", LastName = "Linevich", Avatar = "~/Images/profile-80x80.jpg", Date_of_birth = new DateTime(2003, 03, 20) };
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                    await dbContext.WishList.AddAsync(new WishList { UserID = user.Id });
                }
            }
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                ApplicationUser seller = new() { Email = sellerEmail, UserName = "Beaut1ful", Name = "Vlad", LastName = "Linevich", Avatar = "~/Images/profile-80x80.jpg", Date_of_birth = new DateTime(2003, 03, 20) };
                IdentityResult result = await userManager.CreateAsync(seller, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(seller, "seller");
                    await dbContext.WishList.AddAsync(new WishList { UserID = seller.Id });
                }
            }

            List<Product> Products = new List<Product>
            {
                new Product { UserID = "dcfe9850-7a68-4fff-94a2-228546173d11", CategoryID = 1, ProductTitle = "Good templates", Description = "very very good", ProductImage = "~/Images/itemimage.png", SoftwareVersion = "HTML", Price = 20.00m , CreateDate = DateTime.Now},
                new Product { UserID = "dcfe9850-7a68-4fff-94a2-228546173d11", CategoryID = 2, ProductTitle = "Good JS", Description = "very very good JS", ProductImage = "~/Images/itemimage.png", SoftwareVersion = "JS", Price = 10.00m, CreateDate = DateTime.Now }
            };

            if (dbContext.Product.IsNullOrEmpty())
            {
                foreach (Product product in Products)
                {
                    await dbContext.Product.AddAsync(product);
                }
            }

            List<Review> Reviews = new List<Review>
            {
                new Review { UserID = "dcfe9850-7a68-4fff-94a2-228546173d11", ProductID = 3, Content = "Awesome", Grade = 10, WriteTime = DateTime.Now},
                new Review { UserID = "48043fe3-523f-4c81-acba-17204e0da154", ProductID = 3, Content = "Medium", Grade = 7, WriteTime = DateTime.Now }
            };

            if (dbContext.Review.IsNullOrEmpty())
            {
                foreach (Review review in Reviews)
                {
                    await dbContext.Review.AddAsync(review);
                }
            }

            List<Category> Categories = new List<Category>
            {
                new Category { CategoryName = "Templates" },
                new Category { CategoryName = "JavaScript" },
                new Category { CategoryName = "CSS" },
                new Category { CategoryName = "HTML5" },
                new Category { CategoryName = "Plugins" },
                new Category { CategoryName = "PHP" }
            };

            if(dbContext.Category.IsNullOrEmpty())
            {
                foreach (Category category in Categories)
                {
                    await dbContext.Category.AddAsync(category);
                }
            }

            List<Sale> Sales = new List<Sale>
            {
                new Sale { UserID = "48043fe3-523f-4c81-acba-17204e0da154", ProductID = 3, SaleDate = DateTime.Now }
            };

            if (dbContext.Sale.IsNullOrEmpty())
            {
                foreach (Sale sale in Sales)
                {
                    await dbContext.Sale.AddAsync(sale);
                }
            }

            dbContext.SaveChanges();
        }
    }
}
