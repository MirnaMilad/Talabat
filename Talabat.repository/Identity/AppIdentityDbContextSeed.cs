using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities.Identity;

namespace Talabat.repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any()) { 
            var User = new AppUser()
            {
                DisplayName= "Mirna Milad",
                Email = "mirnamilad0101@gmail.com",
                UserName= "Mirna",
                PhoneNumber = "01013898149"
            };
            await userManager.CreateAsync(User , "P@ssw0rd");
            }

        }
    }
}
