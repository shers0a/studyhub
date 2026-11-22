using Microsoft.AspNetCore.Identity;
using proiect_dotnet.Models;

namespace proiect_dotnet.Data
{
    // Runs at application startup: creates the roles and the admin account if they do not exist.
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("SeedData");

            // TODO (Pas 7): De ce verificam RoleExistsAsync inainte de CreateAsync? (seed-ul ruleaza la fiecare pornire)
            // 1. Create the roles (only if missing, so this is safe to run on every startup).
            foreach (var roleName in new[] { Roles.Admin, Roles.User })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Create the admin account. The password is NOT stored in the code or in Git:
            //    it comes from User Secrets (SeedAdmin:Password).
            // TODO (Pas 7): De ce parola adminului vine din User Secrets si nu este scrisa in cod sau in Git?
            var adminEmail = configuration["SeedAdmin:Email"];
            var adminPassword = configuration["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                logger.LogWarning("SeedAdmin:Email / SeedAdmin:Password are not configured. Admin account was not created.");
                return;
            }

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    DisplayName = "Administrator",
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (!result.Succeeded)
                {
                    logger.LogError("Could not create the admin account: {Errors}",
                        string.Join("; ", result.Errors.Select(e => e.Description)));
                    return;
                }
            }

            // TODO (Pas 7): Ce se intampla daca adminul exista deja, dar nu are rolul Admin?
            if (!await userManager.IsInRoleAsync(admin, Roles.Admin))
            {
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }

            // 3. Accounts created before roles existed (e.g. test accounts) get the User role.
            foreach (var user in userManager.Users.ToList())
            {
                if (user.Id != admin.Id && (await userManager.GetRolesAsync(user)).Count == 0)
                {
                    await userManager.AddToRoleAsync(user, Roles.User);
                }
            }
        }
    }
}
