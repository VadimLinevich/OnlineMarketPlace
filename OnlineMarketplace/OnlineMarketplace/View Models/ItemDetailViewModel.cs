using OnlineMarketplace.Entities;

namespace OnlineMarketplace.View_Models
{
    public class ItemDetailViewModel
    {
        public ApplicationUser user { get; set; }

        public Product product { get; set; }

        public Review review { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }

        public List<WishList> WishLists { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
