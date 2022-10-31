using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.Data;
using courseappchallenge.ViewModels.CourseViewModels;

namespace courseappchallenge.Pages.Courses;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public GetCourseByIdViewModel GetCourseByIdViewModel { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null || _context.Courses == null) return NotFound();

        var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == id);
        if (course == null) return NotFound();

        GetCourseByIdViewModel = course;

        return Page();
    }
}

