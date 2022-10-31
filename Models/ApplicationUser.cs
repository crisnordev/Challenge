using Microsoft.AspNetCore.Identity;

namespace courseappchallenge.Models;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
}
