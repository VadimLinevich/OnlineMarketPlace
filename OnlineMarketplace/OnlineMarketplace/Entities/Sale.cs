using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMarketplace.Entities
{
    [PrimaryKey(nameof(UserID), nameof(ProductID))]
    public class Sale
    {
        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        public int ProductID { get; set; }

        public Product Product { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime SaleDate { get; set; }
    }
}
