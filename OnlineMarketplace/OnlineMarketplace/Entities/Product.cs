using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMarketplace.Entities
{
    public class Product
    {
        public int ProductID { get; set; }

        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        public int? CategoryID { get; set; }

        public Category? Category { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductTitle { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(512)]
        public string ProductImage { get; set; }

        [Required]
        [MaxLength(64)]
        public string SoftwareVersion { get; set; }

        [Required]
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "Enter correct positive value (Max 2 decimal places)")]
        [Range(0.50, 10000.00, ErrorMessage = "Price should be at least 0.50 and at max 10000.00")]
        [Precision(8, 2)]
        public decimal Price { get; set; }

        [Required]
        public int NumberOfSales { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }

        [Required]
        public bool Is_Deleted { get; set; }

        public List<File> Files { get; set; } = new();

        public List<Review> Reviews { get; set; } = new();

        public List<Sale> Sales { get; set; } = new();

        public List<WishList> WishLists { get; set; } = new();
    }
}
