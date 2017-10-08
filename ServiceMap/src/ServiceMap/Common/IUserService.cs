using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ServiceMap.Models.apiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ServiceMap.Common
{
   public interface IUserService
    {
         Task<AppUser> GetUser(ClaimsPrincipal user);
    }
}
