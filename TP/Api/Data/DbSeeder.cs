using Microsoft.AspNetCore.Identity;

namespace Api.Data
{
    public class DbSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            const string clientRole = "client";
            const string technicianRole = "technicien";
            const string adminRole = "admin";
            const string adminEmail = "admin@admin.com";
            const string adminPassword = "Azertyui5!";

            if (!await roleManager.RoleExistsAsync(technicianRole))
            {
                await roleManager.CreateAsync(new IdentityRole(technicianRole));
            }

            if (!await roleManager.RoleExistsAsync(clientRole))
            {
                await roleManager.CreateAsync(new IdentityRole(clientRole));
            }

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
                else
                {
                    throw new Exception("Erreur lors de la création de l'utilisateur admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
