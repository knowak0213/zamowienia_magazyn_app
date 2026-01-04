using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace zamowienia_magazyn_app.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Ensure the database is created
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();

            string[] roleNames = { "Admin", "Client" };
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

            // Create a default Admin user if not exists
            var adminUser = new IdentityUser
            {
                UserName = "admin@zm.pl",
                Email = "admin@zm.pl",
                EmailConfirmed = true
            };

            var _user = await userManager.FindByEmailAsync(adminUser.Email);

            if (_user == null)
            {
                var createPowerUser = await userManager.CreateAsync(adminUser, "Admin123!");
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
