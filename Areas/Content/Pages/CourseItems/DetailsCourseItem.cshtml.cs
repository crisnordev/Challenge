using System.Data.Common;
using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseAppChallenge.Areas.Content.Pages.CourseItems;

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
        if (id == null) return NotFound(new ErrorResultViewModel("CIDT X01 - Id can not be null."));
        CourseItemId = id;

        try
        {
            var courseItem = await _context.CourseItems.AsNoTracking().
                Include(y => y.Course).
                Include(z => z.Lectures).
                FirstOrDefaultAsync(x => x.CourseItemId == id);
            if (courseItem == null) return NotFound(new ErrorResultViewModel("CIDT X02 - Can not find this module."));
            CourseId = courseItem.CourseId;
            GetCourseItemByIdViewModel = courseItem;
            
            return Page();
            
        }
        catch (DbException ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CIDT X03 - Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CIDT X04 - Something is wrong.", ex.Message));
        }
    }
}