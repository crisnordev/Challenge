#nullable enable

using System.Data.Common;
using CourseAppChallenge.Data;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseAppChallenge.Areas.Content.Pages.Courses;

[Authorize]
public class DetailsCourseModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsCourseModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public GetCourseByIdViewModel GetCourseByIdViewModel { get; set; } = default!;

    public Guid? CourseId { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("CODT X01 - Id can not be null."));
        var course = await _context.Courses.AsNoTracking().Include(x => x.CourseItems)
            .FirstOrDefaultAsync(y => y.CourseId == id);
        if (course == null) return NotFound(new ErrorResultViewModel("CODT X02 - Can not find this course."));
        GetCourseByIdViewModel = course;
        CourseId = id;
        
        
        try
        {
            return Page();
        }
        catch (DbException ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CODT X03 - Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CODT X04 - Something is wrong.", ex.Message));
        }
    }
}