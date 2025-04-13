using EventPlannerAPI.Models;
using Microsoft.AspNetCore.Identity;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Rolleri oluştur
        await CreateRolesAsync(roleManager);

        // Admin kullanıcısını oluştur
        await CreateAdminUserAsync(userManager);
    }

    // Rolleri oluşturma işlemi
    private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    // Admin kullanıcısını oluşturma işlemi
    private static async Task CreateAdminUserAsync(UserManager<ApplicationUser> userManager)
    {
        var adminUser = await userManager.FindByEmailAsync("admin@admin.com");

        if (adminUser == null)
        {
            var user = new ApplicationUser
            {
                UserName = "Admin",
                Email = "admin@admin.com",
            };

            var result = await userManager.CreateAsync(user, "Admin123.");

            if (result.Succeeded)
            {
                // Admin rolünü admin kullanıcıya atama
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
