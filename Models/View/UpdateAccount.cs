using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Projekat.Controllers;

namespace Projekat.Models.View {
    public class UpdateModel {
        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string gender { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required]
        [Display(Name = "Username")]
        [RegularExpression(@"^.{6,}$", ErrorMessage = "Username has to have more then 5 characters!")]
        public string username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "Password must contain one letter and one number and minimum 8 characters!")]

        public string password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare (nameof( password), ErrorMessage = "Passwords don't match!")]
        public string confirmPassword { get; set; }

        public bool isUpdate;
    }
}