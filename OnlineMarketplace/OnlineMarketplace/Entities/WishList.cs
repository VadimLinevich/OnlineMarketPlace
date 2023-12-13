using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMarketplace.Entities
{
    public class WishList
    {
        public int WishListID { get; set; }

        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}
