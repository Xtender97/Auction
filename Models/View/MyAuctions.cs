using System.Collections.Generic;
using Projekat.Models.Database;

namespace Projekat.Models.View { 

    public class MyAuctionsModel {
        public IList<Auction> myAuctions;
        public IList<string> images;
        public string pageNumber { get; set;}
        public IList<string> timers;

    }
}