using Microsoft.AspNetCore.Identity;

namespace CourseAppChallenge.Models;

public class AppRole : IdentityRole<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();

    public override string Name { get; set; } = string.Empty;

    public virtual IList<AppUser> AppUsers { get; set; } = new List<AppUser>();
}