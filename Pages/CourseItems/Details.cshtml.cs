using System.Data.Common;
using courseappchallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.ViewModels;
using courseappchallenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;

namespace courseappchallenge.Pages.CourseItems;

[Authorize(Policy = "RequireAdministratorRole")]
[Authorize(Policy = "RequireStudentRole")]
public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public GetCourseItemByIdViewModel GetCourseItemByIdViewModel { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));

        try
        {
            GetCourseItemByIdViewModel = await _context.CourseItems.AsNoTracking().
                Include(y => y.Course).
                Include(z => z.Lectures).
                FirstOrDefaultAsync(x => x.CourseItemId == id);
            
            return Page();
            
        }
        catch (DbException ex)
        {
            return string.IsNullOrEmpty(GetCourseItemByIdViewModel?.CourseItemTitle) ? 
                NotFound(new ErrorResultViewModel("Can not find this module.")) : 
                StatusCode(500, new ErrorResultViewModel("Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("Something is wrong.", ex.Message));
        }
    }
}