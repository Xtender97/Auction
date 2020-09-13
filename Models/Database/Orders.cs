using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Projekat.Models.Database
{



    public class Order
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public DateTime orderDate { get; set; }
        
        [Required]
        public int price { get; set; }

        [Required]
        public int amount { get; set; }

        [Required]
        public string userId { get; set; }

        public User user { get; set; }

    }


    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(order => order.id).ValueGeneratedOnAdd();
        }

    }
}