using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace courseappchallenge.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    public void OnGet()
    {

    }
}
