using BlogSystem.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Repository.Identity
{
    public static class AdminSeed
    {
        public async static Task SeedUserAsync(UserManager<User> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new User()
                {
                    Email = "amrabbas@gmail.com",
                    UserName = "Amr_Abbas",
                    Role = UserRole.Admin                   
                };

                await _userManager.CreateAsync(user, "Sa7eb$ElWebsite");
            }
        }
    }
}
