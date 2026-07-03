using GymManagementSystem.DAL.Models;
using GymManagementSystem.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.DataSeeding
{
    public static class IdentityDataSeeding
    {
    public static async Task SeedIdentityDataAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger logger,
        CancellationToken ct = default
        )

        {
            try
            {
                
                if (!await roleManager.Roles.AnyAsync(ct))
                {
                    var roles = new List<IdentityRole>
            {
                new IdentityRole("SuperAdmin"),
                new IdentityRole("Admin")
            };
                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(role);
                    }
                }
 
                if (!await userManager.Users.AnyAsync(ct))
                {
                    var SuperAdmin = new ApplicationUser()
                    {
                        FirstName = "Omar",
                        LastName = "Essam",
                        UserName = "OmarEssam",
                        Email = "OmarEssam12@gmail.com",
                        EmailConfirmed = true,
                        PhoneNumber = "01000011111"
                    };

                    var result = await userManager.CreateAsync(SuperAdmin, "P@ssw0rd123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            logger.LogError($"{error.Code} : {error.Description}");
                        }
                    }

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Alaa",
                        LastName = "Mohamed",
                        UserName = "AlaaMohamed",
                        Email = "AlaaMohamed12@gmail.com",
                        EmailConfirmed = true,
                        PhoneNumber = "01000011111"
                    };

                    var adminResult = await userManager.CreateAsync(Admin, "P@ssw0rd123");
                    if (adminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(Admin, "Admin");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

    }
}

