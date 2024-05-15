using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sybring_project.Data;
using sybring_project.Models.Db.Email;
using sybring_project.Repos.Interfaces;
using sybring_project.Repos.Services;
using sybring_project.Models.Db;

using Microsoft.AspNetCore.Identity;



//var host = new HostBuilder()
//    .ConfigureFunctionsWebApplication()
//    .ConfigureServices((ctx, services) =>
//    {
//        var connectionString = ctx.Configuration["DefaultConnection"];
//        services.AddDbContext<ApplicationDbContext>(options =>
//        {
//            options.UseSqlServer(connectionString);
//        });
//        services.AddApplicationInsightsTelemetryWorkerService();
//        services.ConfigureFunctionsApplicationInsights();
//        services.AddScoped<ITimeService, TimeService>();
//        services.AddScoped<IEmailSender, EmailSender>();
//    })
//    .Build();


//host.Run();


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((ctx, services) =>
    {
        var connectionString = ctx.Configuration["DefaultConnection"];
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        // Register ASP.NET Core Identity
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<ITimeService, TimeService>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IUserServices, UserServices>();
    })
    .Build();

host.Run();