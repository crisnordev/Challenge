using courseappchallenge.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace courseappchallenge.Areas.Identity.Pages.Account;

public class RoleNamePageModel : PageModel
{
    public SelectList RoleNameSelectList { get; set; } = default!;

    public void PopulateRolesDropDownList(ApplicationDbContext context, object? selectedRole = null)
    {
        var rolesQuery = from item in context.Roles
            orderby item.Name
            select item;

        RoleNameSelectList = new SelectList(rolesQuery.AsNoTracking(),
            "Id", "Name", selectedRole);
    }
}