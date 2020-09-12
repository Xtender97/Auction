using System.Collections.Generic;
using Projekat.Models.Database;
namespace Projekat.Models.View {

    public class AdminApproveModel{ 

        public IList<User> Users { get; set; }

        public IList<Auction> Auctions { get; set; }

        public IList<string> images { get; set; }
    }
}