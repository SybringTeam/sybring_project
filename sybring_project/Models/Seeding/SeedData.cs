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
            if (user is null)
            {
                user = new User
                {
                    UserName = "admin@mail.com",
                    Email = "admin@mail.com",
                    EmailConfirmed = true,

                };
                await userManager.CreateAsync(user, "Admin_2023");
            }
            await userManager.AddToRoleAsync(user, "admin");
            await userManager.AddToRoleAsync(user, "superadmin");


        }

    }
}
