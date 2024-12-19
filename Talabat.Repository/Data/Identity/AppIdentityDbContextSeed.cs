using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Data.Identity
{
    public class AppIdentityDbContextSeed
    {

        public static async Task SeedUserAsync (UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var User = new AppUser()
                {

                    DisplayName="Ismaeel Abulmaaty",
                    Email="ismaeelmatty@gmail.com",
                    UserName= "ismaeelmatty",
                    PhoneNumber="01003793959"

                };

              await  _userManager.CreateAsync(User ,"pa$$w0rd");
            }
        }

    }
}
