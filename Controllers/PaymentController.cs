using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projekat.Models.Database;
using Projekat.Models.View;

namespace Projekat.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private AuctionContext context;
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public PaymentController(AuctionContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult BuyTokens()
        {
            return View("BuyTokens");
        }

        public async Task<string> addPayment(string amount)
        {

            Order order = new Order();
            order.orderDate = DateTime.Now;
            order.amount = int.Parse(amount);
            order.price = int.Parse(amount);
            order.userId = this.userManager.GetUserId(User);
            this.context.order.Add(order);


            User loggedInUser = this.context.Users.Find(this.userManager.GetUserId(User));
            loggedInUser.tokenCount += int.Parse(amount);
            this.context.Users.Update(loggedInUser);
            this.context.SaveChanges();

            await signInManager.RefreshSignInAsync(loggedInUser);


            return "Succesfully created order!";
        }


        public IActionResult Payments(string page){ 

            int pageNumber = int.Parse(page);
            pageNumber++;
            int pageSize = 10;
            
            PaymentsModel model = new PaymentsModel();
            User loggedInUser = this.context.Users.Find(this.userManager.GetUserId(User));
            model.Orders = this.context.order.Where(order => order.userId == loggedInUser.Id).Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
            model.pageNumber = pageNumber;
            return View(model);
        }

    }
}