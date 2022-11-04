using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.Data;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseViewModels;

namespace CourseAppChallenge.Pages.Courses;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public EditCourseViewModel EditCourseViewModel { get; set; } = default!;

    public Course Course { get; set; } = default!;


    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));

        Course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == id);
        if (Course == null) return NotFound(new ErrorResultViewModel("Can not find this course."));

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            var entry = _context.Update(Course);
            entry.CurrentValues.SetValues(EditCourseViewModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return NotFound(!CourseExists(Course.CourseId)
                ? new ErrorResultViewModel("Can not find this course.", ex.Message)
                : new ErrorResultViewModel("Something is wrong.", ex.Message));
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