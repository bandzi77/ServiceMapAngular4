using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceMap.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ServiceMap.Models.apiModels;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceMap.Common;
using MimeKit;
using System.IO;
using MimeKit.Utils;
using static ServiceMap.Common.Enums;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
//UserManager<AppUser> userMgr, RoleManager<IdentityRole> roleMgr 
namespace ServiceMap.Controllers.apiControllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Superusers")]
    public class UsersController : Controller
    {
        private SignInManager<AppUser> signInManager;
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IConfiguration configuration;
        private string roleSuperUser;
        private string roleUser;
        private IEmailService emailService;
        private IUserService currentUser;
        private readonly string superUser;

        public UsersController(SignInManager<AppUser> signinMgr, UserManager<AppUser> userMgr,
            RoleManager<IdentityRole> roleMgr, IConfiguration config, IEmailService emailService, IUserService userService)
        {
            signInManager = signinMgr;
            userManager = userMgr;
            roleManager = roleMgr;
            configuration = config;
            this.emailService = emailService;
            this.currentUser = userService;
            roleSuperUser = configuration["Data:Roles:Superuser"];
            roleUser = configuration["Data:Roles:User"];
            superUser = configuration["Data:FirstUser:Email"];
        }


        // GET: api/values
        [HttpGet("GetUsers")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetUsers(string email, bool showLockedOnly)
        {
            IQueryable<AppUser> _users = null;

            var userRole = await roleManager.FindByNameAsync(roleSuperUser);

            if (!String.IsNullOrWhiteSpace(email))
            {
                _users = userManager.Users.Where(x => x.NormalizedEmail.StartsWith(email.ToUpper()));
            }
            else
            {
                _users = userManager.Users;
            }

            var result_ = _users
                .Select(x => new User()
                {
                    _id = x.Id,
                    TntUserName = x.TntUserName,
                    Email = x.Email,
                    LimitOfRequestsPerDay = x.LimitOfRequestsPerDay,
                    NumberOfRequestsPerDay = x.NumberOfRequestsPerDay,
                    IsSuperUser = x.Roles.Any(y => y.RoleId == userRole.Id),
                    IsLocked = x.LockoutEnd > DateTime.Now && x.LockoutEnabled
                }).OrderBy(s => s.TntUserName).AsQueryable();

            if (showLockedOnly)
            {
                result_ = result_.Where(x => x.IsLocked == showLockedOnly);
            }
            result_ = result_.Where(x => x.Email.ToUpper() != superUser.ToUpper() || superUser.ToUpper() == null);
            var result = new { users = result_, paging = "" };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            IdentityResult resultIdent = null;
            var result = new { success = false, message = ConstsData.UserModelInvalid };

            if (ModelState.IsValid && user._id == "0" && user.Email.ToUpper() != superUser.ToUpper())
            {
                var validationResult = BaseValidationOfUser(user);

                if (!validationResult.Success)
                {
                    return Ok(validationResult);
                }

                // Sprawdza czy taki użytkownik już istnieje
                if (await userManager.FindByEmailAsync(user.Email.ToUpper()) != null)
                {
                    result = new { success = false, message = ConstsData.UserAlreadyExists };
                    return Ok(result);
                }

                // Tworzy role jeśli ich nie ma w systemie - rozważyć usięnięcie tego kawałka kodu
                if (await roleManager.FindByNameAsync(roleSuperUser) == null)
                {
                    resultIdent = await roleManager.CreateAsync(new IdentityRole(roleSuperUser));
                }

                if (await roleManager.FindByNameAsync(roleUser) == null)
                {
                    resultIdent = await roleManager.CreateAsync(new IdentityRole(roleUser));
                }

                if (resultIdent != null && !resultIdent.Succeeded)
                {
                    result = new { success = false, message = ConstsData.UserCreateAnotherError };
                    return Ok(result);
                }

                // Tworzenie użytkownika
                AppUser newUser = new AppUser
                {
                    TntUserName = user.TntUserName.Trim(),
                    UserName = user.Email.Trim().ToUpper(),
                    Email = user.Email.Trim().ToUpper(),
                    AccessFailedCount = 5,
                    LockoutEnd = user.IsLocked ? DateTimeOffset.MaxValue : (DateTimeOffset?)null,
                    LimitOfRequestsPerDay = user.LimitOfRequestsPerDay,
                    NumberOfRequestsPerDay = user.NumberOfRequestsPerDay
                };

                resultIdent = await userManager.CreateAsync(newUser, user.Password);

                if (!resultIdent.Succeeded)
                {
                    result = new { success = false, message = ConstsData.UserCreateIdentityError };
                    return Ok(result);
                }

                // Dodawanie do roli
                if (user.IsSuperUser)
                {
                    resultIdent = await userManager.AddToRoleAsync(newUser, roleSuperUser);
                }
                else
                {
                    resultIdent = await userManager.AddToRoleAsync(newUser, roleUser);
                }

                // Zwraca wynik końcowy operacji
                if (resultIdent.Succeeded)
                {
                    // Sprawdza czy email został wysłany do użytkownika po założeniu konta
                    if (SendDataToNewUser(user))
                    {
                        result = new { success = true, message = ConstsData.UserCreateSuccess };
                    }
                    else
                    {
                        result = new { success = false, message = ConstsData.UserCreateEmailError };
                    }
                }
                else
                {
                    await userManager.DeleteAsync(newUser);
                    result = new { success = false, message = ConstsData.UserCreateAnotherError };
                }
            }
            return Ok(result);
        }


        // Wysyła dane konta dla nowego użytkownika
        private bool SendDataToNewUser(User newUser)
        {
            var fromEmail = currentUser.GetUser(User).Result.NormalizedEmail;

            return emailService.SendEmail("TNT SM", null, newUser.Email,
                ConstsData.PasswordForNewUserSubject,
                ConstsData.PasswordForNewUserMsg + $"{newUser.Password}" +
                ConstsData.PasswordForNewUserQueryLimit + $"{newUser.LimitOfRequestsPerDay}"+
                ConstsData.PasswordForNewUserLinkApp+
                ConstsData.PasswordForNewMessageFooter,
                EmailFormat.html, true);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] User user)
        {
            var result = new { success = false, message = ConstsData.UpdateUserError };

            if (user.Email.ToUpper() != superUser.ToUpper())
            {
                IdentityResult resultIdent = null;
                ModelState.Remove("Password");
                if (ModelState.IsValid && user._id != "0")
                {
                    var validationResult = BaseValidationOfUser(user);

                    if (!validationResult.Success)
                    {
                        return Ok(validationResult);
                    }
                    
                    var userToUpdate = await userManager.FindByIdAsync(id);
                    if (userToUpdate != null)
                    {
                        userToUpdate.TntUserName = user.TntUserName.Trim();
                        userToUpdate.LimitOfRequestsPerDay = user.LimitOfRequestsPerDay;
                        userToUpdate.LockoutEnd = user.IsLocked ? DateTimeOffset.MaxValue : (DateTimeOffset?)null;
                        resultIdent = await userManager.UpdateAsync(userToUpdate);
                        if (resultIdent.Succeeded)
                        {
                            if (!user.IsLocked)
                            {
                                resultIdent = await userManager.ResetAccessFailedCountAsync(userToUpdate);
                            }

                            if (resultIdent.Succeeded)
                            {
                                if (await userManager.IsInRoleAsync(userToUpdate, roleSuperUser) && user.IsSuperUser)
                                {
                                    result = new { success = true, message = ConstsData.UpdateUserSuccess };
                                }
                                else
                                {
                                    dynamic res = await UserUpdateRole(userToUpdate, user);
                                    result = new { success = (bool)res.success, message = (string)res.message };
                                }
                            }
                            else
                            {
                                result = new { success = false, message = ConstsData.UpdateUserResetAccessFailed };
                            }
                        }
                        else
                        {
                            result = new { success = false, message = ConstsData.UpdateUserIdentity };
                        }
                    }
                    else
                    {
                        result = new { success = false, message = ConstsData.UpdateUserNotExists };
                    }
                }
            }

            return Ok(result);
        }

        private Result BaseValidationOfUser(User user)
        {
            var result = new Result { Success = true };
            // Sprawdza, czy użytkownik spełnia warunek dot. limitów
            if (!user.IsSuperUser && (user.LimitOfRequestsPerDay == null || user.LimitOfRequestsPerDay < 1 || user.LimitOfRequestsPerDay > 500))
            {
                result = new Result { Success = false, Message = ConstsData.UserCreateInvalidLimit };
                return result;
            }

            // Sprawdza, czy użytkownik jako administrator posiada właściwą domenę.
            if (user.IsSuperUser && !user.Email.ToUpper().Trim().EndsWith("TNT.COM"))
            {
                result = new Result { Success = false, Message = ConstsData.UserCreateInvalidAdminEmail };
                return result;
            }

            return result;
        }

        private async Task<Object> UserUpdateRole(AppUser appUser, User user)
        {
            string roleToRemove = user.IsSuperUser ? roleUser : roleSuperUser;
            string roleToSet = user.IsSuperUser ? roleSuperUser : roleUser;

            IdentityResult resultIdent = null;

            if (await userManager.IsInRoleAsync(appUser, roleToRemove))
            {
                resultIdent = await userManager.RemoveFromRoleAsync(appUser, roleToRemove);
            }

            if (!await userManager.IsInRoleAsync(appUser, roleToSet))
            {
                resultIdent = await userManager.AddToRoleAsync(appUser, roleToSet);
            }


            if (resultIdent == null || resultIdent.Succeeded)
            {
                return new { success = true, message = ConstsData.UpdateUserSuccess };
            }
            else
            {
                return new { success = false, message = ConstsData.UpdateUserRoleIdentity };
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityResult resultIdent = null;
            var result = new { success = false, message = ConstsData.DeleteUserError };

            if (id != "0")
            {
                var user = await userManager.FindByIdAsync(id);
                if (user.Email.ToUpper() != configuration["Data:FirstUser:Email"].ToUpper())
                {
                    if (user != null)
                    {
                        resultIdent = await userManager.DeleteAsync(user);
                        if (resultIdent.Succeeded)
                        {
                            result = new { success = true, message = ConstsData.DeleteUserSuccess };
                        }
                    }
                }
            }

            return Ok(result);
        }

        private class Result {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
    }
}
