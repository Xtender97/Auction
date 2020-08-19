using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Projekat.Models.Database{
    public class AuctionContext : IdentityDbContext<User>{
        public AuctionContext( DbContextOptions options) : base(options) {}
    
        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);

            builder.ApplyConfiguration (new IdentityRoleConfiguration());

        }


    }
}