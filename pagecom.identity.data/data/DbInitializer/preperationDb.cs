using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using pagecom.identity.data.configinformation;
using pagecom.identity.data.data.user;

namespace pagecom.identity.data.data.DbInitializer;

public class preperationDb
{
    public static async void DatabaseCreate(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            ApplicationDbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>()!;
            UserManager<ApplicationUser> userManager  = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>()!;
            RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>()!;
            await SeedInformation(context,userManager,roleManager);
        }
    }
    
    private static async Task SeedInformation(ApplicationDbContext context,UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager)
    {
        Console.WriteLine("appling migrations");   
        Console.WriteLine("add admin user");
        var value = await roleManager.FindByNameAsync(Config.Admin);
        if (value == null)
        {
            roleManager.CreateAsync(new IdentityRole(Config.Admin)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(Config.Cusotmer)).GetAwaiter().GetResult();
        }
        else
        {
            return;
        }
        // admin user
        ApplicationUser adminUser = new ApplicationUser()
        {
            UserName = "shavendragoesoft@gmail.com",
            Email = "shavendragoesoft@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "0702638887",
            FirstName = "shavendra",
            LastName = "Ekanayake",
        };
        
        Console.WriteLine("add admin user password");
        var result =userManager.CreateAsync(adminUser, "Shave90-").GetAwaiter().GetResult();
        userManager.AddToRoleAsync(adminUser, Config.Admin).GetAwaiter().GetResult();

        Console.WriteLine("add admin user claims");
        userManager.AddClaimsAsync(adminUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, adminUser.UserName),
            new Claim(JwtClaimTypes.Role, Config.Admin),
            new Claim(JwtClaimTypes.GivenName,adminUser.FirstName),
        }).GetAwaiter().GetResult();
        
        
        ApplicationUser customerUser = new ApplicationUser()
        {
            UserName = "shvavendraekanayake@gmail.com",
            Email = "shvavendraekanayake@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "0702638887",
            FirstName = "shavendra",
            LastName = "customer",
        };

        userManager.CreateAsync(customerUser, "Shave90-").GetAwaiter().GetResult();
        userManager.AddToRoleAsync(customerUser, Config.Cusotmer).GetAwaiter().GetResult();

        userManager.AddClaimsAsync(customerUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, customerUser.UserName),
            new Claim(JwtClaimTypes.Role, Config.Cusotmer),
            new Claim(JwtClaimTypes.GivenName,customerUser.FirstName),
        }).GetAwaiter().GetResult();

    }
}