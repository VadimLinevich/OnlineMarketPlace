using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMarketplace.Entities
{
    [PrimaryKey(nameof(UserID), nameof(ProductID))]
    public class Review
    {
        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        public int ProductID { get; set; }

        public Product Product { get; set; }

        [Required]
        [MaxLength(512)]
        public string Content { get; set; }

        [Required]
        public int Grade { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime WriteTime { get; set; }
    }
}
