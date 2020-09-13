using System;
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


        public BidController(AuctionContext context,  SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;



        }


        public async Task<string> bid(string auctionId, string oldPrice, string increment){ 
            User loggedInUser = await this.userManager.GetUserAsync(base.User);
            Auction auction = await this.context.auction.FindAsync(int.Parse(auctionId));

            Bid bid = new Bid();
            bid.auctionId = int.Parse(auctionId);
            bid.UserId = loggedInUser.Id;
            bid.timestamp = DateTime.Now;
            bid.oldPrice = float.Parse(oldPrice);
            bid.increment = float.Parse(increment);
            bid.newPrice = bid.oldPrice + bid.increment;

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