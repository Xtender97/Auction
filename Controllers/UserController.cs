using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekat.Models.Database;
using Projekat.Models.View;

namespace Projekat.Controllers
{

    public class UserController : Controller
    {
        private AuctionContext context;

        private UserManager<User> userManager;

        private SignInManager<User> signInManager;

        private IMapper mapper;

        public UserController(AuctionContext context, UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.signInManager = signInManager;
        }


        public IActionResult isEmailUnique(string email)
        {
            bool exist = this.context.Users.Where(user => user.Email == email).Any();

            if (exist)
            {
                return Json("Email already taken");
            }
            else
            {
                return Json(true);
            }

        }

        public IActionResult isUserNameUnique(string username)
        {
            bool exist = this.context.Users.Where(user => user.UserName == username).Any();

            if (exist)
            {
                return Json("Username already taken");
            }
            else
            {
                return Json(true);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = this.mapper.Map<User>(model);
            user.tokenCount = 0;

            IdentityResult result = await this.userManager.CreateAsync(user, model.password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            result = await this.userManager.AddToRoleAsync(user, Roles.user.Name);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");



        }


        public IActionResult Register()
        {
            return View();
        }


        public IActionResult Login(string returnUrl)
        {
            LoginModel model = new LoginModel() { returnUrl = returnUrl };
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User user = await this.context.Users.Where(u => u.UserName == model.username).SingleAsync();

            if (user.isDeleted)
            {
                ModelState.AddModelError("", "Your account is deleted!");
                return View(model);

            }
            var result = await this.signInManager.PasswordSignInAsync(model.username, model.password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }



            if (model.returnUrl != null)
                return Redirect(model.returnUrl);
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }


        public async Task<IActionResult> Logout()
        {

            await this.signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getUsersToApprove()
        {
            string signedInUser = this.userManager.GetUserId(User);
            List<User> users = await this.context.Users.Where(u => u.Id != signedInUser).ToListAsync();
            AdminApproveModel model = new AdminApproveModel();
            model.Users = users;
            return View("AdminApprove", model);

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getAuctionsToApprove()
        {
            List<Auction> auctions = await this.context.auction.Where(u => u.state == State.DRAFT).ToListAsync();
            List<string> images = new List<string>();
            foreach (Auction auction in auctions)
            {
                string image = Convert.ToBase64String(auction.image);
                images.Add(image);
            }
            AdminApproveModel model = new AdminApproveModel();
            model.Auctions = auctions;
            model.images = images;
            return View("AdminApproveAuctions", model);

        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> deleteUser(string id)
        {
            User user = await this.context.Users.Where(u => u.Id == id).SingleAsync();
            user.isDeleted = true;
            this.context.Users.Update(user);

            List<Auction> auctions = this.context.auction.Where(a=> a.userId == id && (a.state == State.READY ||a.state == State.OPEN)).ToList();

            foreach(Auction auction in auctions){
                auction.state = State.EXPIRED;
                this.context.auction.Update(auction);
            }
            await this.context.SaveChangesAsync();

            return await getUsersToApprove();
        }



    }



}