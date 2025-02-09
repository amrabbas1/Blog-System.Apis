using BlogSystem.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user, UserManager<User> userManager);
    }
}
