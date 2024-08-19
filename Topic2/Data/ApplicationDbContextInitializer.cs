using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Topic2.Domain.Constants;
using Topic2.Domain.Entities;

namespace Topic2.Data
{

    public static class InitializerExtensions
    {
        public static async Task InitializeAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
                await initializer.InitializeAsync();
                await initializer.SeedAsync();
            }
        }
    }
    public class ApplicationDbContextInitializer
    {

        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<ApplicationDbContextInitializer> logger;

        public ApplicationDbContextInitializer(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to initialize the database" + ex);
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TryseedAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to seed the database" + ex);
            }
        }

        public async Task TryseedAsync()
        {
            #region Init Administrator
            try
            {
                var administratorRole = new IdentityRole(Roles.Administactor);
                if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
                {
                    await roleManager.CreateAsync(administratorRole);
                }

                var adminstator = new User { UserName = "administrator", Email = "administrator@localhost" };
                if (userManager.Users.All(u => u.UserName != adminstator.UserName))
                {
                    await userManager.CreateAsync(adminstator, "Administrator1!");
                    await userManager.AddToRolesAsync(adminstator, [administratorRole.Name]);
                }

            }
            catch (Exception ex)
            {
                logger.LogError("Failed to seed the database" + ex);
            }
            #endregion
        }

    }
}