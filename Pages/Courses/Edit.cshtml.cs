using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseAppChallenge.Pages.Courses;

[Authorize(Policy = "RequireAdministratorRole")]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public EditCourseViewModel EditCourseViewModel { get; set; } = default!;

    public Guid Id { get; set; }


    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));
        Id = (Guid)id;

        var course = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(x => x.CourseId == id);
        if (course == null) return NotFound(new ErrorResultViewModel("Can not find this course."));

        EditCourseViewModel = course;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (!ModelState.IsValid) return Page();
        
        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound(new ErrorResultViewModel("Can not find this course."));

        try
        {
            await TryUpdateModelAsync(
                course!,
                "editcourseviewmodel", 
                c => c.CourseTitle, c => c.Tag, c => c.Summary,
                c => c.DurationInMinutes);

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ErrorResultViewModel("Internal server error.",
                ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("Something is wrong.",
                ex.Message));
        }
    }

    private bool CourseExists(Guid id)
    {
        return (_context.Courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
    }
}