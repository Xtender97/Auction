
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Projekat.Models.Database {

    public static class Roles {
        public static IdentityRole admin = new IdentityRole(){
            Name = "Admin", 
            NormalizedName = "ADMIN"
        
        };
        public static IdentityRole user = new IdentityRole(){
            Name = "User", 
            NormalizedName = "USER"
        
        };
    }

    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                Roles.user,
                Roles.admin
            );
        }
    }
}