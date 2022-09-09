using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using pagecom.identity.data.data.user;

namespace Pagecom.identity.mainApp.ProfileChange;

public class ProfileServiceclass : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ProfileServiceclass(IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        string sub = context.Subject.GetSubjectId();
        ApplicationUser user = await _userManager.FindByIdAsync(sub);
        var userclaim = await _userClaimsPrincipalFactory.CreateAsync(user);
        
        List<Claim> claims = userclaim.Claims.ToList();
        claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
        
        claims.Add(new Claim(JwtClaimTypes.Name,user.UserName)); // delete this line 
        claims.Add(new Claim(JwtClaimTypes.FamilyName,user.FirstName)); // delete this line 
        
        
        if (_userManager.SupportsUserRole)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role,role));
                claims.Add(new Claim(JwtClaimTypes.Name,user.UserName));
                claims.Add(new Claim(JwtClaimTypes.FamilyName,user.FirstName));
                if (_roleManager.SupportsRoleClaims)
                {
                    IdentityRole rolex = await _roleManager.FindByNameAsync(role);
                    if (rolex != null)
                    {
                        claims.AddRange(await _roleManager.GetClaimsAsync(rolex));
                    }
                }
            }
        }
        
        context.IssuedClaims = claims;
        
        // var user = await _userManager.GetUserAsync(context.Subject);
        //
        // var claims = new List<Claim>
        // {
        //     new Claim("FullName", user.UserName),
        //     new Claim("load","shavendraekanayake")
        // };
        //
        // context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        // string sub = context.Subject.GetSubjectId();
        // ApplicationUser user = await _userManager.FindByNameAsync(sub);
        // context.IsActive = user != null;

        
        var user = await _userManager.GetUserAsync(context.Subject);
        
        context.IsActive = (user != null);
    }
}