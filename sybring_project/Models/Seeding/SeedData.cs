using Microsoft.AspNetCore.Identity;
using sybring_project.Data;
using sybring_project.Models.Db;

namespace sybring_project.Models.Seeding
{
    public class SeedData
    {

        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await SeedRoles(roleManager);
            await SeedAdmin(userManager);
        }

        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.RoleExistsAsync("superadmin") is not true)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "superadmin" });
            }
            if (await roleManager.RoleExistsAsync("admin") is not true)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "admin" });
            }
            if (await roleManager.RoleExistsAsync("underconsult") is not true)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "underconsult" });
            }
        }
        private async static Task SeedAdmin(UserManager<User> userManager)
        {
            var user = await userManager.FindByEmailAsync("admin@mail.com");
            var useradmin = await userManager.FindByEmailAsync("henrik.sorin@Sybring.com");
            var useradmin2 = await userManager.FindByEmailAsync("edan@mail.com");
            var useradmin3 = await userManager.FindByEmailAsync("dawood@mail.com");
            if (useradmin is null)
            {
                useradmin = new User
                {
                    UserName = "henrik.sorin@Sybring.com",
                    Email = "henrik.sorin@Sybring.com",
                    EmailConfirmed = true,
                    FirstName = "Henrik",
                    LastName = "Soring"
                };
                await userManager.CreateAsync(useradmin, "Admin_2024");
            }

            if (useradmin2 is null)
            {
                useradmin2 = new User
                {
                    UserName = "edan@mail.com",
                    Email = "edan@mail.com",
                    EmailConfirmed = true,

                };
                await userManager.CreateAsync(useradmin2, "Admin_2024");
            }
            if (useradmin3 is null)
            {
                useradmin3 = new User
                {
                    UserName = "dawood@mail.com",
                    Email = "dawood@mail.com",
                    EmailConfirmed = true,

                };
                await userManager.CreateAsync(useradmin3, "Admin_2024");
            }
            if (user is null)
            {
                user = new User
                {
                    UserName = "admin@mail.com",
                    Email = "admin@mail.com",
                    EmailConfirmed = true,

                };
                await userManager.CreateAsync(user, "Admin_2024");
            }
            await userManager.AddToRoleAsync(useradmin, "admin");
            await userManager.AddToRoleAsync(useradmin2, "admin");
            await userManager.AddToRoleAsync(useradmin3, "admin");
            await userManager.AddToRoleAsync(user, "superadmin");

        }
    }
}
