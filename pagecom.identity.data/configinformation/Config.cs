using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace pagecom.identity.data.configinformation;

public class Config
{
    
    public const string Admin = "admin";
    public const string Cusotmer = "customer";
    public static IEnumerable<Client> Clients => new List<Client>
    {
        
        
        
        new Client()
        {
            ClientId = "api_access",
            AllowedGrantTypes = GrantTypes.ClientCredentials,   
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },
            AllowedScopes = { "bookaccess" }
        },
        new Client()
        {
            ClientId = "mvc_client",
            AllowedGrantTypes = GrantTypes.Code,
            AllowRememberConsent = false,
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email
            },
            RedirectUris = new List<string>()
            {
                "https://localhost:7500/signin-oidc"
            },
            PostLogoutRedirectUris = new List<string>()
            {
                "https://localhost:7500/signout-callback-oidc" // this thing create new logout page 
            },
            
            AlwaysSendClientClaims = true,
            AlwaysIncludeUserClaimsInIdToken = true
        }
    };
    
    
    public static IEnumerable<ApiScope> Scopes => new List<ApiScope>()
    {
      new ApiScope("bookaccess","book api")  
    };

    public static IEnumerable<IdentityResource> Resources => new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email()
    };
    
    
    public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1234",
                Username = "shavendra",
                Password = "shave90-",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName, "shavendra"),
                    new Claim(JwtClaimTypes.FamilyName, "Ekanayake")
                }
            }
        };
}