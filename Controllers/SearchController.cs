using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Projekat.Models.Database;
using Projekat.Models.View;

namespace Projekat.Controllers
{
    public class SearchController : Controller
    {
        AuctionContext context;

        public SearchController(AuctionContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var model = new SearchModel();
            model.state = "ALL";
            return View(model);
        }

        public IActionResult SearchAuctions(string keyWord, string minPrice, string maxPrice, string state, string page)
        {
            if (minPrice == null)
            {
                minPrice = "0";
            }
            if (maxPrice == null)
            {
                maxPrice = "999999999999";
            }

            var auctionsQuery = this.context.auction
                .Where(item => item.startPrice > float.Parse(minPrice))
                .Where(item => item.startPrice < float.Parse(maxPrice));

            if (keyWord != null)
            {
                auctionsQuery = auctionsQuery.Where(item => item.name.Contains(keyWord));
            }

            if (state != "ALL")
            {
                State? stateType = null;
                switch (state)
                {
                    case "DRAFT":
                        stateType = State.DRAFT;
                        break;

                    case "OPEN":
                        stateType = State.OPEN;
                        break;

                    case "READY":
                        stateType = State.READY;
                        break;
                    case "EXPIRED":
                        stateType = State.EXPIRED;
                        break;
                    case "SOLD":
                        stateType = State.SOLD;
                        break;
                }
                auctionsQuery = auctionsQuery.Where(item => item.state == stateType);
            }
            int pageNumber = 1;
            if (page != "undefined")
            {
                pageNumber = int.Parse(page);
            }
            int pageSize = 12;

            auctionsQuery = auctionsQuery.OrderBy(x => x.creationDate);
            auctionsQuery = auctionsQuery.Skip((pageNumber - 1) * pageSize);

            List<Auction> auctions = auctionsQuery.Take(pageSize).ToList();

            List<string> images = new List<string>();
            List<string> timers = new List<string>(); 

            foreach (Auction auction in auctions)
            {
                string image = Convert.ToBase64String(auction.image);
                images.Add(image);
                if(auction.openingDate < DateTime.Now && auction.state == State.DRAFT ){
                    auction.state = State.OPEN;
                    this.context.auction.Update(auction);
                }
                if(auction.closingDate < DateTime.Now && auction.state == State.OPEN){
                    // MAKE IT SOLD OR EXPIRED DEPENDING ON THE BIDDING
                }
                if(auction.state == State.OPEN){ 
                    TimeSpan difference = auction.closingDate - DateTime.Now;
                    string strDiff = String.Format("{0}:{1}:{2}", 
                        difference.Days *24 + difference.Hours, 
                        difference.Minutes,                     
                        difference.Seconds);

                    timers.Add(strDiff);
                }
                else {
                    timers.Add(null);
                }
            }

            


            this.context.SaveChangesAsync();

            var model = new MyAuctionsModel();

            model.myAuctions = auctions;
            model.images = images;
            model.pageNumber = pageNumber.ToString();
            model.timers = timers;
            return PartialView(model);
        }
    }
}