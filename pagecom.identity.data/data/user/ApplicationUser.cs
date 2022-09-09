using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;

namespace pagecom.identity.data.data.user;

public class ApplicationUser : IdentityUser
{
    public string  FirstName { get; set; }
    public string LastName { get; set; }
}