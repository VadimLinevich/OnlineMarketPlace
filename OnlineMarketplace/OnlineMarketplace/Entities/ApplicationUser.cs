using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineMarketplace.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MinLength(3)]
        [MaxLength(64)]
        public string? Name { get; set; }

        [MinLength(3)]
        [MaxLength(64)]
        public string? LastName { get; set; }

        [MaxLength(512)]
        public string? Avatar { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date_of_birth { get; set; }

        public List<Product> Products { get; set; } = new();

        public List<Review> Reviews { get; set; } = new();

        public List<Sale> Sales { get; set; } = new();

        public WishList WishList { get; set; }
    }
}
