using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Projekat.Models.View
{

    public class SearchModel
    {

        [Display(Name = "Key word")]
        public string keyWord { get; set; }

        [Display(Name = "Minimum price")]
        public float? minPrice { get; set;}

        [Display(Name = "Maximum price")]
        public float? maxPrice { get; set;}

        [Display(Name = "Auction state")]
        public string state { get; set;}

        public List<SelectListItem> states = new List<SelectListItem>{
            new SelectListItem { Value = "ALL", Text ="ALL"},
            new SelectListItem { Value = "DRAFT", Text = "DRAFT" },
            new SelectListItem { Value = "READY", Text = "READY" },
            new SelectListItem { Value = "OPEN", Text = "OPEN" },
            new SelectListItem { Value = "EXPIRED", Text = "EXPIRED" },
            new SelectListItem { Value = "SOLD", Text = "SOLD" }


     };


    }
}