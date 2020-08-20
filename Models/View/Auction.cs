using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Projekat.Models.View {

    public class AuctionModel { 

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Required]
        [Display(Name = "Image")]
        public IFormFile image { get; set; }


        [Required]
        [Display(Name = "Price")]
        public float startPrice { get; set; }

        [Required]
        [Display(Name = "When to open")]
        [DataType(DataType.DateTime)]
        public DateTime openingDate { get; set; }

        [Required]
        [Display(Name = "When to close")]
        [DataType(DataType.DateTime)]
        public DateTime closingDate { get; set; }



    }
 }

