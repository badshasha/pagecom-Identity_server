using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using pagecom.identity.data.configinformation;
using pagecom.identity.data.data.user;

namespace pagecom.identity.data.data.DbInitializer;

public  class IdentityDbInitializer : Initializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityDbInitializer(ApplicationDbContext context,UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    public void Initilizer()
    {
        if (_roleManager.FindByNameAsync(Config.Admin).Result != null)
        {
            _roleManager.CreateAsync(new IdentityRole(Config.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Config.Cusotmer)).GetAwaiter().GetResult();
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

        _userManager.CreateAsync(adminUser, "shave90-").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(adminUser, Config.Admin).GetAwaiter().GetResult();

        _userManager.AddClaimsAsync(adminUser, new Claim[]
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

        _userManager.CreateAsync(customerUser, "shave90-").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(customerUser, Config.Cusotmer).GetAwaiter().GetResult();

        _userManager.AddClaimsAsync(customerUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, customerUser.UserName),
            new Claim(JwtClaimTypes.Role, Config.Cusotmer),
            new Claim(JwtClaimTypes.GivenName,customerUser.FirstName),
        }).GetAwaiter().GetResult();
    }
}