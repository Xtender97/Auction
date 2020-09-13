using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekat.Models.Database;
using Projekat.Models.View;

namespace Projekat.Controllers
{
    [Authorize]
    public class AuctionController : Controller
    {

        private AuctionContext context;

        private IMapper mapper;

        private SignInManager<User> signInManager;

        private UserManager<User> userManager;


        public AuctionController(AuctionContext context, IMapper mapper, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.signInManager = signInManager;
            this.userManager = userManager;

        }

        // FILE TYPE CHECKER HELPER METOD

        private bool IsValidFileType(string fileName)
        {
            string[] extensions = { "jpeg", "jpg", "png" };

            foreach (string extension in extensions)
            {
                if (fileName.EndsWith(extension))
                {
                    return true;
                }
            }
            return false;
        }

        //  POST CREATE AUCTION

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuction(AuctionModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.image == null)
            {
                ModelState.AddModelError("", "Image is required!");
                ViewData["Mode"] = "create";
                return View(model);

            }

            string fileName = model.image.FileName;

            if (!IsValidFileType(fileName))
            {
                ViewData["Mode"] = "create";
                ModelState.AddModelError("", "Unsuported file type!");
                return View(model);
            }


            User loggedInUser = await this.userManager.GetUserAsync(base.User);

            Auction auction = this.mapper.Map<Auction>(model);

            using (BinaryReader reader = new BinaryReader(model.image.OpenReadStream()))
            {

                auction.image = reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length));
            };

            auction.creationDate = DateTime.Now;
            auction.priceIncrement = 0;
            auction.state = State.DRAFT;
            auction.user = loggedInUser;
            auction.userId = loggedInUser.Id;

            if (auction.creationDate >= auction.openingDate)
            {
                ModelState.AddModelError("", "Auction can't start in the past!!!");
                ViewData["Mode"] = "create";
                return View(model);
            }

            if (auction.closingDate <= auction.openingDate)
            {
                ModelState.AddModelError("", "Auction can't end before it starts!!!");
                ViewData["Mode"] = "create";
                return View(model);
            }

            await this.context.auction.AddAsync(auction);

            await this.context.SaveChangesAsync();

            return Redirect("/Auction/MyAuctions");



        }

        // GET CREATE AUCTION

        public IActionResult CreateAuction()
        {
            ViewData["Mode"] = "create";
            return View();
        }

        // GET EDIT AUCTION 

        public async Task<IActionResult> EditAuction(int id)
        {

            ViewData["Mode"] = "update";
            Auction auction = await this.context.auction.Where(a => a.id == id).FirstAsync();
            AuctionModel model = this.mapper.Map<AuctionModel>(auction);
            return View("CreateAuction", model);
        }

        //  POST EDIT AUCTION

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAuction(AuctionModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("CreateAuction", model);
            }

            User loggedInUser = await this.userManager.GetUserAsync(base.User);

            Auction auction = this.mapper.Map<Auction>(model);

            if (model.newImage != null)
            {
                string fileName = model.newImage.FileName;

                if (!IsValidFileType(fileName))
                {
                    ModelState.AddModelError("", "Unsuported file type!");
                    return View("CreateAuction", model);
                }

                using (BinaryReader reader = new BinaryReader(model.newImage.OpenReadStream()))
                {

                    auction.image = reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length));
                };

            }

            if (auction.creationDate >= auction.openingDate)
            {
                ModelState.AddModelError("", "Auction can't start in the past!!!");
                return View("CreateAuction", model);

            }

            if (auction.closingDate <= auction.openingDate)
            {
                ModelState.AddModelError("", "Auction can't end before it starts!!!");
                return View("CreateAuction", model);

            }

            Auction auctionDB = await this.context.auction.SingleAsync(auctionDB => auctionDB.id == auction.id);

            auctionDB.name = auction.name;
            auctionDB.description = auction.description;
            auctionDB.startPrice = auction.startPrice;
            auctionDB.openingDate = auction.openingDate;
            auctionDB.closingDate = auction.closingDate;
            if (auction.image != null)
            {
                auctionDB.image = auction.image;
            }




            this.context.auction.Update(auctionDB);

            await this.context.SaveChangesAsync();

            return RedirectToAction(nameof(AuctionController.MyAuctions), "Auction");

        }

        // DELETE AUCTION 

        public async Task<IActionResult> DeleteAuction(int id)
        {
            var auction = await this.context.auction.SingleAsync(auction => auction.id == id);
            auction.state = State.DELETED;
            this.context.auction.Update(auction);
            await this.context.SaveChangesAsync();
            return RedirectToAction(nameof(AuctionController.MyAuctions), "Auction");

        }

        // GET ALL MY AUCTIONS

        public async Task<IActionResult> MyAuctions()
        {
            User loggedInUser = await this.userManager.GetUserAsync(base.User);

            IList<Auction> auctions = await this.context.auction.
                Where(item => item.userId == loggedInUser.Id).ToListAsync();

            IList<string> images = new List<string>();

            foreach (Auction auction in auctions)
            {
                string image = Convert.ToBase64String(auction.image);
                images.Add(image);
            }

            MyAuctionsModel model = new MyAuctionsModel()
            {
                myAuctions = auctions,
                images = images
            };

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveAuction(int id){
            var auction = await this.context.auction.SingleAsync(auction => auction.id == id);
            auction.state = State.READY;
            this.context.auction.Update(auction);
            await this.context.SaveChangesAsync();
            return RedirectToAction(nameof(UserController.getAuctionsToApprove), "User");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuctionAdmin(int id){
            var auction = await this.context.auction.SingleAsync(auction => auction.id == id);
            auction.state = State.DELETED;
            this.context.auction.Update(auction);
            await this.context.SaveChangesAsync();
            return RedirectToAction(nameof(UserController.getAuctionsToApprove), "User");
        }


     
    }
}