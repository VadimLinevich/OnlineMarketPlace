﻿using OnlineMarketplace.Entities;

namespace OnlineMarketplace.View_Models
{
    public class ItemsViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        public IEnumerable<Sale> Sales { get; set; }

        public IEnumerable<Entities.File> Files { get; set; }

        public List<WishList> WishLists { get; set; }
    }
}
