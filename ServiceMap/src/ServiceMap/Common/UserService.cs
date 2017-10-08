using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ServiceMap.Models.apiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ServiceMap.Common
{
    public class UserService : IUserService
    {
        private UserManager<AppUser> userManager;
        public UserService(UserManager<AppUser> uMngr)
        {
            userManager = uMngr;
        }
        public async Task<AppUser> GetUser(ClaimsPrincipal user)
        {
            if (user == null)
            { return null; }

            var currentUser = await userManager.GetUserAsync(user);
            return currentUser;
        }
    }
}
