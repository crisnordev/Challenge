using Microsoft.AspNetCore.Identity;

namespace courseappchallenge.Models;

public class ApplicationRole : IdentityRole
{
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
    
}