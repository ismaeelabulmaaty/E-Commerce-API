using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.Extentions
{
    public static class UserManagerExtentions
    {


        public static async Task<AppUser> FindUserWithAddressByEmail( this UserManager<AppUser> userManager , ClaimsPrincipal User)
        {

            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var user = userManager.Users.Include(u=>u.Address).FirstOrDefault(u=>u.NormalizedEmail == email.ToLower());

            return user;

        }

    }
}
