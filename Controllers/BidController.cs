using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projekat.Models.Database;

namespace Projekat.Controllers
{


    public class BidController : Controller
    {

        private AuctionContext context;

        private SignInManager<User> signInManager;

        private UserManager<User> userManager;


        public BidController(AuctionContext context, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;



        }


        public async Task<string> bid(string auctionId, string oldPrice, string increment)
        {
            User loggedInUser = await this.userManager.GetUserAsync(base.User);
            if (loggedInUser == null)
            {
                return "Error: Login to be able to bid!!!";
            }

            if (loggedInUser.tokenCount <= 0)
            {
                return "Error: No tokens!!!";
            }
            Auction auction = await this.context.auction.FindAsync(int.Parse(auctionId));

            if (auction.userId == loggedInUser.Id)
            {
                return "Error: You cant bid on your own auction!";
            }

            if(auction.state != State.OPEN)
            {
                return "Error: You cant bid on not OPEN auction!!!";
            }

            Bid lastBid = this.context.bid.Where(bid => bid.auctionId == int.Parse(auctionId)).OrderByDescending(bid => bid.timestamp).FirstOrDefault();

            if (lastBid != null)
            {
                float oldPriceFormated = (float)Math.Round(float.Parse(oldPrice) * 1000f )/1000f;
                float newPriceFormated = (float)Math.Round(lastBid.newPrice*1000f)/1000f;
                if (Math.Abs(newPriceFormated -  oldPriceFormated) > newPriceFormated * 0.02 ){
                    return "Error: Someone bid before you!!!";
                }
            }

            if((auction.closingDate - DateTime.Now).TotalSeconds < 10 ){
                auction.closingDate = auction.closingDate.AddSeconds(10);
            }

            Bid bid = new Bid();
            bid.auctionId = int.Parse(auctionId);
            bid.UserId = loggedInUser.Id;
            bid.timestamp = DateTime.Now;
            bid.oldPrice = (float)Math.Round(float.Parse(oldPrice) * 1000f )/1000f;
            bid.increment = (float)Math.Round(float.Parse(increment) * 1000f )/1000f;
            bid.newPrice = (float)Math.Round((bid.oldPrice + bid.increment) * 1000f )/1000f;

            auction.priceIncrement += bid.increment;
            loggedInUser.tokenCount--;

            this.context.auction.Update(auction);
            this.context.Users.Update(loggedInUser);


            this.context.bid.Add(bid);

            this.context.SaveChanges();
            await signInManager.RefreshSignInAsync(loggedInUser);
            return "Succesfull bid";
        }
    }
}