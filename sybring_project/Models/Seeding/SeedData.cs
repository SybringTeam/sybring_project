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
            var project = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var status = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


            await SeedRoles(roleManager);
            await SeedAdmin(userManager);
            await SeedProject(project);
            await SeedStatus(status);

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
            if (await roleManager.RoleExistsAsync("archive") is not true)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "archive" });
            }
        }
        private async static Task SeedAdmin(UserManager<User> userManager)
        {
            var user = await userManager.FindByEmailAsync("admin@mail.com");
            var useradmin = await userManager.FindByEmailAsync("henrik.sorin@Sybring.com");
            var useradmin2 = await userManager.FindByEmailAsync("thomas.carlsson@sybring.com");
            var useradmin3 = await userManager.FindByEmailAsync("edan@mail.com");
            var useradmin4 = await userManager.FindByEmailAsync("dawood@mail.com");
            var useradmin5 = await userManager.FindByEmailAsync("spurti@mail.com");
            var useradmin6 = await userManager.FindByEmailAsync("posh@mail.com");
            var userarchive = await userManager.FindByEmailAsync("archive@mail.com");
            var realuser = await userManager.FindByEmailAsync("dawood.rizwan@outlook.com");
            if (useradmin is null)
            {
                useradmin = new User
                {
                    UserName = "henrik.sorin@Sybring.com",
                    Email = "henrik.sorin@Sybring.com",
                    EmailConfirmed = true,
                    FirstName = "Henrik",
                    LastName = "Sorin"
                };
                await userManager.CreateAsync(useradmin, "Admin_2024");
            }
            if (useradmin2 is null)
            {
                useradmin2 = new User
                {
                    UserName = "thomas.carlsson@sybring.com",
                    Email = "thomas.carlsson@sybring.com",
                    EmailConfirmed = true,
                    FirstName = "Thomas",
                    LastName = "Carlsson"
                };
                await userManager.CreateAsync(useradmin2, "Admin_2024");
            }
            if (useradmin3 is null)
            {
                useradmin3 = new User
                {
                    UserName = "edan@mail.com",
                    Email = "edan@mail.com",
                    EmailConfirmed = true,
                    FirstName = "Edan",
                    LastName = "Beardan"

                };
                await userManager.CreateAsync(useradmin3, "Admin_2024");
            }
            if (useradmin4 is null)
            {
                useradmin4 = new User
                {
                    UserName = "dawood@mail.com",
                    Email = "dawood@mail.com",
                    EmailConfirmed = true,
                    FirstName = "Dawood",
                    LastName = "Rizwan"

                };
                await userManager.CreateAsync(useradmin4, "Admin_2024");
            }
            if (useradmin5 is null)
            {
                useradmin5 = new User
                {
                    UserName = "spurti@mail.com",
                    Email = "spurti@mail.com",
                    EmailConfirmed = true,
                    FirstName = "Spurti",
                    LastName = "Salimath"

                };
                await userManager.CreateAsync(useradmin5, "Admin_2024");
            }
            if (useradmin6 is null)
            {
                useradmin6 = new User
                {
                    UserName = "posh@mail.com",
                    Email = "posh@mail.com",
                    EmailConfirmed = true,
                    FirstName = "Natalie",
                    LastName = "Aktas"

                };
                await userManager.CreateAsync(useradmin6, "Admin_2024");
            }
            if (user is null)
            {
                user = new User
                {
                    UserName = "admin@mail.com",
                    Email = "admin@mail.com",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Adminsson"

                };
                await userManager.CreateAsync(user, "Admin_2024");
            }
            if (userarchive is null)
            {
                userarchive = new User
                {
                    UserName = "archive@mail.com",
                    Email = "archive@mail.com",
                    EmailConfirmed = true,
                    FirstName = "Archive",
                    LastName = "Archivesson"

                };
                await userManager.CreateAsync(userarchive, "Admin_2024");
            }
            if (realuser is null)
            {

                realuser = new User
                {
                    UserName = "dawood.rizwan@outlook.com",
                    Email = "dawood.rizwan@outlook.com",
                    EmailConfirmed = true,
                    FirstName = "Real",
                    LastName = "User"

                };
                await userManager.CreateAsync(realuser, "Admin_2024");
            }
            await userManager.AddToRoleAsync(useradmin, "admin");
            await userManager.AddToRoleAsync(useradmin2, "admin");
            await userManager.AddToRoleAsync(useradmin3, "admin");
            await userManager.AddToRoleAsync(useradmin4, "admin");
            await userManager.AddToRoleAsync(useradmin5, "admin");
            await userManager.AddToRoleAsync(useradmin6, "admin");
            await userManager.AddToRoleAsync(user, "superadmin");
            await userManager.AddToRoleAsync(userarchive, "archive");
            await userManager.AddToRoleAsync(realuser, "underconsult");

        }
        private async  static Task SeedProject(ApplicationDbContext project)
        {
            if(project.Projects.Any() == false) 
            {
                await project.Projects.AddRangeAsync(
                    new Project
                    {
                        Name = "Siemens"
                    },
                    new Project
                    {
                        Name = "Väderstad"
                    }, 
                    new Project
                    {
                        Name = "Hydro"
                    }, 
                    new Project
                    {
                        Name = "Jordbruksverket"
                    }, 
                    new Project
                    {
                        Name = "Migrationsverket"
                    }
                    );
                    await project.SaveChangesAsync();
            }
        }

        private async static Task SeedStatus(ApplicationDbContext status)
        {
            if (!status.Status.Any())
            {
                await status.Status.AddRangeAsync(
                    new Status { Name = "active" },
                    new Status { Name = "inactive" },
                    new Status { Name = "inprogress" },
                    new Status { Name = "Admin"}
                );
                await status.SaveChangesAsync();
            }
        }
    }
}
