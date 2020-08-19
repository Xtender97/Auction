using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Projekat.Models.View { 


    public class LoginModel { 
        [Required]
        [Display(Name = "Username")]
        public string username { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [HiddenInput]
        public string returnUrl { get; set; }
    }
}