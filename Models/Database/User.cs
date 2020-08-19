using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Projekat.Models.Database
{
    public class User : IdentityUser
    {

        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string gender { get; set; }




    }

    // public class UserConfiguration : IEntityTypeConfiguration<User>{ 
    //     public void Configure(EntityTypeBuilder<User> builder){ 
    //         builder.Property( user => user.id).ValueGeneratedOnAdd();
    //         builder.HasData( new User () { 
    //             id = 1, firstName = "Milan",
    //              lastName = "Boskovic", email = "jovbosko1@gmail.com", 
    //              password= "1", gender = "male", username = "Xtender"});
    //     }
    // }
}