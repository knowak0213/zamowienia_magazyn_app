using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace zamowienia_magazyn_app.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();

            string[] roleNames = { "Admin", "Client" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
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

            if (!context.Products.Any())
            {
                var products = new Models.Product[]
                {
                    new Models.Product { Name = "Komputer", Price = 2500, StockQuantity = 10, Description = "Wydajny komputer stacjonarny" },
                    new Models.Product { Name = "Myszka", Price = 50, StockQuantity = 50, Description = "Myszka optyczna" },
                    new Models.Product { Name = "Klawiatura", Price = 120, StockQuantity = 30, Description = "Klawiatura mechaniczna" },
                    new Models.Product { Name = "Głośniki", Price = 200, StockQuantity = 20, Description = "Głośniki stereo" },
                    new Models.Product { Name = "Monitor", Price = 800, StockQuantity = 15, Description = "Monitor 24 cale" },
                    new Models.Product { Name = "Słuchawki", Price = 150, StockQuantity = 40, Description = "Słuchawki nauszne z mikrofonem" },
                    new Models.Product { Name = "Podkładka", Price = 20, StockQuantity = 100, Description = "Podkładka pod mysz" }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
