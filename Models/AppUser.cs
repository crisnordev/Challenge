#nullable enable
using Microsoft.AspNetCore.Identity;

namespace CourseAppChallenge.Models;

public class AppUser : IdentityUser<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();

    public override string UserName { get; set; } = string.Empty;

    public override string NormalizedUserName => UserName.ToUpper();

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public Guid? AppRoleId { get; set; }

    public virtual AppRole? AppRole { get; set; }
}