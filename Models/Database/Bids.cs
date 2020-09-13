using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Projekat.Models.Database
{

    public class Bid
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public int auctionId { get; set; }

        public Auction Auction { get; set; }

        [Required]
        public DateTime timestamp { get; set; }

        public float oldPrice { get; set; }

        public float newPrice { get; set; }

        public float increment { get; set; }

    }

    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.Property(bid => bid.id).ValueGeneratedOnAdd();
        }

    }
}