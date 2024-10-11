using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using BlogData;
using BlogWeb.Models;
using BlogData.Entities;

namespace BlogWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateRolesAndUsers(host.Services).Wait();
            host.Run();
        }

        private static async Task CreateRolesAndUsers(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Author", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create a default Admin user
            ApplicationUser user = await userManager.FindByEmailAsync("admin@blog.com");

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = "admin@blog.com",
                    Email = "admin@blog.com"
                };
                await userManager.CreateAsync(user, "Admin@123");
            }

            await userManager.AddToRoleAsync(user, "Admin");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        // Configure DbContext
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

                        // Configure Identity
                        services.AddDefaultIdentity<ApplicationUser>(options =>
                        {
                            options.SignIn.RequireConfirmedAccount = true;
                            options.Password.RequireDigit = true;
                            options.Password.RequireLowercase = true;
                            options.Password.RequireNonAlphanumeric = true;
                            options.Password.RequireUppercase = true;
                            options.Password.RequiredLength = 6;
                            options.Password.RequiredUniqueChars = 1;
                            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                            options.Lockout.MaxFailedAccessAttempts = 5;
                            options.Lockout.AllowedForNewUsers = true;
                            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                            options.User.RequireUniqueEmail = true;
                        })
                        .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();

                        // Configure MVC and Razor Pages
                        services.AddControllersWithViews();
                        services.AddRazorPages();


                        // Configure cookie settings
                        services.ConfigureApplicationCookie(options =>
                        {
                            options.Cookie.HttpOnly = true;
                            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                            options.LoginPath = $"/Account/Login";
                            options.AccessDeniedPath = $"/Account/AccessDenied";
                            options.SlidingExpiration = true;
                        });

                        // Configure routing
                        services.AddMvc(options =>
                        {
                            options.EnableEndpointRouting = false;
                        });
                    })
                    .Configure((context, app) =>
                    {
                        // Configure middleware
                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }
                        else
                        {
                            app.UseExceptionHandler("/Home/Error");
                            app.UseHsts();
                        }

                        app.UseHttpsRedirection();
                        app.UseStaticFiles();
                        app.UseRouting();
                        app.UseAuthentication();
                        app.UseAuthorization();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllerRoute(
                                name: "default",
                                pattern: "{controller=Home}/{action=Index}/{id?}");
                            endpoints.MapRazorPages();
                        });
                    });
                });

    }
}
