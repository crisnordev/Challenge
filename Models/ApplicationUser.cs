using Microsoft.AspNetCore.Identity;

namespace CourseAppChallenge.Models;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
}
