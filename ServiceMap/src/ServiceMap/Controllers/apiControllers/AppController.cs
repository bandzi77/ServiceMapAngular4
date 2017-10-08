using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ServiceMap.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using ServiceMap.Models.apiModels;
using ServiceMap.Common;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceMap.Controllers.apiControllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AppController : Controller
    {
        IConfiguration configuration;
        private SignInManager<AppUser> signInManager;
        private IUserService userService; 

        public AppController(IConfiguration config, SignInManager<AppUser> signinMgr, IUserService usrService)
        {
            configuration = config;
            signInManager = signinMgr;
            userService = usrService;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            bool isSuperUser = false;
            string userEmail = userService.GetUser(User).Result.NormalizedEmail;

            if (User.IsInRole(configuration["Data:Roles:Superuser"]))
            {
                isSuperUser = true;
            }

            var result = new { isSuperUser, userEmail };

            return Ok(result);
        }
    }
}
