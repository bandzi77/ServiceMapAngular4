using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ServiceMap.Models.apiModels
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }


        public static async Task CreateAdminAccount(IServiceProvider serviceProvider,
           IConfiguration configuration)
        {

            UserManager<AppUser> userManager =
                serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string tntUserName = configuration["Data:FirstUser:Name"];
            string email = configuration["Data:FirstUser:Email"];
            string password = configuration["Data:FirstUser:Password"];
            string roleSuperUser = configuration["Data:Roles:Superuser"];
            string roleUser = configuration["Data:Roles:User"];

            if (await userManager.FindByEmailAsync(email) == null)
            {
                if (await roleManager.FindByNameAsync(roleSuperUser) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleSuperUser));
                }

                if (await roleManager.FindByNameAsync(roleUser) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleUser));
                }

                AppUser user = new AppUser
                {
                    UserName = email,
                    TntUserName = tntUserName,
                    Email = email,
                    AccessFailedCount = 100
                };

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleSuperUser);
                }

            }
        }
    }
}
