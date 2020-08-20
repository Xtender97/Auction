using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Projekat.Models.Database
{
    public class AuctionContext : IdentityDbContext<User>
    {
        public DbSet<Auction> auction { get; set; }

        public AuctionContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Auction>().Property(auction => auction.state).HasConversion(new EnumToStringConverter<State>());
            builder.ApplyConfiguration(new AuctionConfiguration());
            builder.ApplyConfiguration(new IdentityRoleConfiguration());

        }


    }
}