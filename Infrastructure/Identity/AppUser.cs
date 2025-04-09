using Microsoft.AspNetCore.Identity;

namespace Infrastucture.Identity;
public class AppUser : IdentityUser
{
    public string EmpCode { get; set; } = string.Empty;
}