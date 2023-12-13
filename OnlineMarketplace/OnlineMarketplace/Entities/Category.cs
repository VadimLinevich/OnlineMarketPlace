using System.ComponentModel.DataAnnotations;

namespace OnlineMarketplace.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(64)]
        public string CategoryName { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}
