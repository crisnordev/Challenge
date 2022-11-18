using System.Data.Common;
using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseAppChallenge.Pages.Courses.CourseItems;

[Authorize]
public class DetailsCourseItemModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsCourseItemModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public GetCourseItemByIdViewModel GetCourseItemByIdViewModel { get; set; } = default!;
    
    public Guid? CourseItemId { get; set; }
    
    public Guid? CourseId { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));
        CourseItemId = id;

        try
        {
            GetCourseItemByIdViewModel = await _context.CourseItems.AsNoTracking().
                Include(y => y.Course).
                Include(z => z.Lectures).
                FirstOrDefaultAsync(x => x.CourseItemId == id);
            CourseId = GetCourseItemByIdViewModel.CourseId;
            
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