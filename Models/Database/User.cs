using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekat.Models.View;

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

        public bool isActive { get; set; }

        public bool isDeleted { get; set; }

        public int tokenCount { get; set; }

        public ICollection<Auction> auctions { get; set; }

        public ICollection<Order> orders { get; set; }

        public ICollection<Bid> Bids { get; set; }





    }

    public class UserProfile : Profile {
        public UserProfile(){
            base.CreateMap<RegisterModel, User>()
            .ForMember (
                destination => destination.Email,
                options => options.MapFrom( data => data.email)
            )
             .ForMember (
                destination => destination.UserName,
                options => options.MapFrom( data => data.username)
            ).ReverseMap();
        }
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