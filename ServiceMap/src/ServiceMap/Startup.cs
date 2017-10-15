using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ServiceMap.Models.apiModels;
using ServiceMap.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using Microsoft.AspNetCore.Identity;
using ServiceMap.Common;
using ServiceMap.Models.Service_Tnt;
namespace ServiceMap
{
    public class Startup
    {
        IConfiguration Configuration;

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange:true)
                .AddEnvironmentVariables()
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO str 214
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration["Data:ConnectionStrings:DbServiceMap"]));
            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(Configuration["Data:ConnectionStrings:DbServiceMapIndentity"]));
            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
    
            services.AddIdentity<AppUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                //opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = true;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = true;
                opts.Lockout.MaxFailedAccessAttempts = 5;
                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(365);

                // Cookie settings
                opts.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(1);
                opts.Cookies.ApplicationCookie.CookieName = "MyTntSMCookie";
                opts.Cookies.ApplicationCookie.CookieHttpOnly = true;
                opts.Cookies.ApplicationCookie.SlidingExpiration = true;

                opts.Cookies.ApplicationCookie.LoginPath = new PathString("/Account/Login");
                opts.Cookies.ApplicationCookie.LogoutPath = new PathString("/Account/Logout");
                opts.Cookies.ApplicationCookie.AccessDeniedPath = new PathString("/Account/Logout");//new PathString("/Account/AccessDenied");
                opts.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
                opts.Cookies.ApplicationCookie.AutomaticChallenge = true;
                opts.Cookies.ApplicationCookie.AuthenticationScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
                opts.Cookies.ApplicationCookie.ReturnUrlParameter = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.ReturnUrlParameter;

                //opts.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                //{
                //    OnRedirectToLogin = ctx =>
                //    {
                //        if (ctx.Request.Path.StartsWithSegments("/api") &&
                //            ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                //        {
                //            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //        }
                //        else
                //        {
                //            ctx.Response.Redirect(ctx.RedirectUri);
                //        }
                //        return Task.FromResult(0);
                //    }
                //};
                // Do zastanowienia nad inną obsługą.
                //OnRedirectToAccessDenied = ctx =>
                //{
                //    if (ctx.Request.Path.StartsWithSegments("/api") &&
                //        ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                //    {
                //        ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                //    }
                //    else
                //    {
                //        ctx.Response.Redirect(ctx.RedirectUri);
                //    }
                //    return Task.FromResult(0);
                //}


            })
                // W przypadku innej ścieżki niż domyślna
                //(opt=>opt.Cookies.ApplicationCookie.LoginPath="/")
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(config => {

                config.Cookies.ApplicationCookie.Events =
                new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        return Task.FromResult(0);
                    }
                };
            });

            //services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddSingleton(Configuration);
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddScoped<UserManager<AppUser>>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddTransient<IServiceTntRepository, ServiceTntRepository>();
            services.AddTransient<IDepotRepository, DepotRepository>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Odwłuje się do appsettings.json -> Logging
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBrowserLink();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseIdentity();
            AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
            //app.UseCookieAuthentication(new CookieAuthenticationOptions()
            //{
            //    AuthenticationScheme = "MyCookieMiddlewareInstance",
            //    LoginPath = new PathString("/Account/Login/"),
            //    AccessDeniedPath = new PathString("/Account/Forbidden/"),
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true
            //});

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //name: "Error",
                //template: "Error",
                //defaults: new { controller = "Error", action = "Error" });

                //routes.MapRoute(
                //  name: "infoPanel",
                //  template: "{controller=Account}/{action=InfoPanel}/{message}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");

                routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
