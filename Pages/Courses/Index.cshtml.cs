using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.Data;
using courseappchallenge.Models;

namespace courseappchallenge.Pages.Courses;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Course> Courses { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Courses != null)
        {
            Courses = await _context.Courses.ToListAsync();
        }
    }
}