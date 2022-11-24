using System.Data.Common;
using CourseAppChallenge.Data;
using CourseAppChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseAppChallenge.Areas.Content.Pages.Courses;

[Authorize(Policy = "RequireAdministratorRole")]
public class DeleteCourseModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteCourseModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public DeleteCourseViewModel DeleteCourseViewModel { get; set; } = default!;

    public Guid? CourseId { get; set; } 
    
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("CODL X01 - Id can not be null."));
        CourseId = id;

        var course = await _context.Courses.AsNoTracking().Include(y => y.CourseItems)
            .FirstOrDefaultAsync(x => x.CourseId == id);
        if (course == null) return NotFound(new ErrorResultViewModel("CODL X02 - Can not find course."));

        DeleteCourseViewModel = course!;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null) return BadRequest(new ErrorResultViewModel("CODL X03 - Id can not be null."));
        var course = await _context.Courses.FindAsync(id); 
        if (course == null) return NotFound(new ErrorResultViewModel("CODL X04 - Can not find course."));
        
        try
        {
            _context.Courses.Remove(course!);
            await _context.SaveChangesAsync();

            return RedirectToPage("./IndexCourse");
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CODL X05 - Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CODL X06 - Something is wrong.", ex.Message));
        }
    }
}