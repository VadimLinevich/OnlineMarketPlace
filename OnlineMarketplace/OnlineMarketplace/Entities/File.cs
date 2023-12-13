using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMarketplace.Entities
{
    public class File
    {
        public int FileID { get; set; }

        public int ProductID { get; set; }

        public Product Product { get; set; }

        [Required]
        [MaxLength(100)]
        public string FileTitle { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
