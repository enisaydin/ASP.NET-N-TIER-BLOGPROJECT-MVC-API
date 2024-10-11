// BlogData/SeedData.cs
using BlogData;
using BlogData.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        context.Database.EnsureCreated(); // Veritabanı oluşturulmuşsa tekrar oluşturmaz

        // Roller
        if (!roleManager.Roles.Any())
        {
            var roles = new[] { "Admin", "Author", "User" };
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Kullanıcılar
        if (!userManager.Users.Any())
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var authorUser = new ApplicationUser
            {
                UserName = "author@example.com",
                Email = "author@example.com",
                EmailConfirmed = true
            };
            result = await userManager.CreateAsync(authorUser, "Author@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(authorUser, "Author");
            }

            var regularUser = new ApplicationUser
            {
                UserName = "user@example.com",
                Email = "user@example.com",
                EmailConfirmed = true
            };
            result = await userManager.CreateAsync(regularUser, "User@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }
    }
}
