using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Projekat.Models.Database;

namespace Projekat.Factories{
    public class ClaimFactory : UserClaimsPrincipalFactory<User>
    {
        private UserManager<User> userManager;

        public ClaimFactory(UserManager<User> userManager, IOptions<IdentityOptions> options): base (userManager, options){
            this.userManager = userManager;
        }
        public override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            return base.CreateAsync(user);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {

            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim (
                new Claim("fullName", user.firstName + " " + user.lastName)
            );

            var roles = await this.userManager.GetRolesAsync( user);
            foreach (var role in roles){
                identity.AddClaim(
                    new Claim (ClaimTypes.Role, role)
                );
            }

            return identity;
            
        }
    }
}