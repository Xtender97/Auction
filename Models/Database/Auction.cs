using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekat.Models.View;

namespace Projekat.Models.Database
{

    public enum State
    {
        DRAFT, READY, OPEN, SOLD, EXPIRED, DELETED
    }

    public class Auction
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string description { get; set; }

        [Required]
        public byte[] image { get; set; }

        [Required]
        public float startPrice { get; set; }

        [Required]
        public DateTime openingDate { get; set; }

        [Required]
        public DateTime creationDate { get; set; }

        [Required]
        public DateTime closingDate { get; set; }

        public float priceIncrement { get; set; }

        [Required]
        public State state { get; set; }

        public User user { get; set; }

        [Required]
        public string userId { get; set; }

        public ICollection<Bid> Bids { get; set; }



    }

    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.Property(auction => auction.id).ValueGeneratedOnAdd();
        }

    }

    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            base.CreateMap<AuctionModel, Auction>()
            .ForMember(
                destination => destination.image,
            options => options.Ignore());
      

            base.CreateMap<Auction, AuctionModel>()
            .ForMember(
                destination => destination.image,
            options => options.Ignore());
        
        }
    }
}