using Microsoft.AspNetCore.Identity;

namespace CourseAppChallenge.Models;

public class ApplicationRole : IdentityRole
{
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
    
}