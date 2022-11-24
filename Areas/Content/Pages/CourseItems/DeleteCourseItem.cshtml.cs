using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseAppChallenge.Areas.Content.Pages.CourseItems;

[Authorize(Policy = "RequireAdministratorRole")]
public class DeleteCourseItemModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteCourseItemModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public DeleteCourseItemViewModel DeleteCourseItemViewModel { get; set; } = default!;

    public Guid? CourseItemId { get; set; }

    public Guid? CourseId { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("CIDE X 01 - Id can not be null."));
        CourseItemId = id;

        var courseItem = await _context.CourseItems.AsNoTracking().Include(y => y.Course)
            .FirstOrDefaultAsync(x => x.CourseItemId == id);
        if (courseItem == null) return NotFound(new ErrorResultViewModel("CIDE X 02 - Can not find this module."));
        DeleteCourseItemViewModel = courseItem;
        CourseId = courseItem.CourseId;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        var courseItem = await _context.CourseItems.FirstOrDefaultAsync(x => x.CourseItemId == id);
        if (courseItem == null) return NotFound(new ErrorResultViewModel("CIDE X 03 - Can not find this module."));
        
        try
        {
            _context.CourseItems.Remove(courseItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Courses/DetailsCourse", new {id = courseItem.CourseId});
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CIDE X 04 - Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CIDE X 05 - Something is wrong.", ex.Message));
        }
    }
}