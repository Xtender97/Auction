using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            State? stateType = null;

            if (state != "ALL")
            {
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
            List<string> lastBidders = new List<string>();
            List<Auction> modifideAuctions = new List<Auction>();

            foreach (Auction auction in auctions)
            {

                if (auction.openingDate <= DateTime.Now.AddSeconds(1) && auction.state == State.READY)
                {
                    auction.state = State.OPEN;
                    this.context.auction.Update(auction);
                    if (stateType != null && auction.state != stateType)
                    {
                        continue;
                    }
                }
                Bid lastBid = this.context.bid.Where(bid => bid.auctionId == auction.id).Include(bid => bid.User).OrderByDescending(bid => bid.timestamp).FirstOrDefault();
                if (auction.closingDate <= DateTime.Now.AddSeconds(1) && auction.state == State.OPEN)
                {

                    // MAKE IT SOLD OR EXPIRED DEPENDING ON THE BIDDING
                    if (lastBid == null)
                    {
                        auction.state = State.EXPIRED;
                    }
                    else
                    {
                        auction.state = State.SOLD;
                        auction.winner = lastBid.UserId;
                    }
                    this.context.auction.Update(auction);
                    if (stateType != null && auction.state != stateType)
                    {
                        continue;
                    }


                }

                modifideAuctions.Add(auction);

                if (lastBid == null)
                {
                    lastBidders.Add(null);
                }
                else
                {
                    lastBidders.Add(lastBid.User.UserName);
                }

                string image = Convert.ToBase64String(auction.image);
                images.Add(image);

                if (auction.state == State.OPEN)
                {
                    TimeSpan difference = auction.closingDate - DateTime.Now;
                    string strDiff = String.Format("{0}:{1}:{2}",
                        difference.Days * 24 + difference.Hours,
                        difference.Minutes,
                        difference.Seconds);

                    timers.Add(strDiff);
                }
                else
                {
                    timers.Add(null);
                }
            }




            this.context.SaveChanges();

            var model = new MyAuctionsModel();

            model.myAuctions = modifideAuctions;
            model.images = images;
            model.pageNumber = pageNumber.ToString();
            model.timers = timers;
            model.lastBidders = lastBidders;
            return PartialView(model);
        }

        public IActionResult DetailsPage(string id)
        {

            Auction auction = this.context.auction.Where(a => a.id == int.Parse(id)).FirstOrDefault();
            List<Bid> bids = this.context.bid.Where(b => b.auctionId == int.Parse(id)).Include(bid => bid.User).Take(10).OrderByDescending(bid => bid.timestamp).ToList();

            if (auction.openingDate <= DateTime.Now.AddSeconds(1) && auction.state == State.READY)
            {
                auction.state = State.OPEN;
                this.context.auction.Update(auction);
            }

            if (auction.closingDate <= DateTime.Now.AddSeconds(1) && auction.state == State.OPEN)
            {

                // MAKE IT SOLD OR EXPIRED DEPENDING ON THE BIDDING
                if (bids.Count <= 0)
                {
                    auction.state = State.EXPIRED;
                }
                else
                {
                    auction.winner = bids.First().UserId;
                    auction.state = State.SOLD;
                }
                this.context.auction.Update(auction);

            }

            this.context.SaveChanges();
            DetailsModel model = new DetailsModel();

            model.auction = auction;
            model.image = Convert.ToBase64String(auction.image);
            model.timer = null;

            if (auction.state == State.OPEN)
            {
                TimeSpan difference = auction.closingDate - DateTime.Now;
                string strDiff = String.Format("{0}:{1}:{2}",
                    difference.Days * 24 + difference.Hours,
                    difference.Minutes,
                    difference.Seconds);

                model.timer = strDiff;
            }


            List<string> usernames = new List<string>();

            foreach (var bid in bids)
            {
                usernames.Add(bid.User.UserName);
            }

            model.bidders = usernames;

            return View(model);


        }

        public IActionResult ReloadDetailsPage(string id)
        {
            Auction auction = this.context.auction.Where(a => a.id == int.Parse(id)).FirstOrDefault();
            List<Bid> bids = this.context.bid.Where(b => b.auctionId == int.Parse(id)).Include(bid => bid.User).OrderByDescending(bid => bid.timestamp).Take(10).ToList();

            if (auction.openingDate <= DateTime.Now.AddSeconds(1) && auction.state == State.READY)
            {
                auction.state = State.OPEN;
                this.context.auction.Update(auction);
            }

            if (auction.closingDate <= DateTime.Now.AddSeconds(1) && auction.state == State.OPEN)
            {

                // MAKE IT SOLD OR EXPIRED DEPENDING ON THE BIDDING
                if (bids.Count <= 0)
                {
                    auction.state = State.EXPIRED;
                }
                else
                {
                    auction.winner = bids.First().UserId;
                    auction.state = State.SOLD;
                }
                this.context.auction.Update(auction);

            }

            this.context.SaveChanges();
            DetailsModel model = new DetailsModel();

            model.auction = auction;
            model.image = Convert.ToBase64String(auction.image);
            model.timer = null;

            if (auction.state == State.OPEN)
            {
                TimeSpan difference = auction.closingDate - DateTime.Now;
                string strDiff = String.Format("{0}:{1}:{2}",
                    difference.Days * 24 + difference.Hours,
                    difference.Minutes,
                    difference.Seconds);

                model.timer = strDiff;
            }


            List<string> usernames = new List<string>();

            foreach (var bid in bids)
            {
                usernames.Add(bid.User.UserName);
            }

            model.bidders = usernames;

            return PartialView("Details", model);
        }


    }


}