using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseAppChallenge.Pages.CourseItems;

[Authorize(Policy = "RequireAdministratorRole")]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] 
    public EditCourseItemViewModel EditCourseItemViewModel { get; set; } = default!;

    public Guid Id { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));

        Id = (Guid)id;

        var courseItem = await _context.CourseItems.AsNoTracking().FirstOrDefaultAsync(x => x.CourseItemId == id);
        if (courseItem == null) return NotFound(new ErrorResultViewModel("Can not find this module."));

        EditCourseItemViewModel = courseItem;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (!ModelState.IsValid) return Page();
        
        var courseItem = await _context.CourseItems.FirstOrDefaultAsync(x => x.CourseItemId == id);
        if (courseItem == null) return NotFound(new ErrorResultViewModel("Can not find this module."));

        try
        {
            await TryUpdateModelAsync(
                courseItem!,
                "editcourseitemviewmodel", 
                c => c.CourseItemTitle, c => c.Order);

            _context.CourseItems.Update(courseItem);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("./Index");
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ErrorResultViewModel("Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("Something is wrong.", ex.Message));
        }
    }

    private bool CourseItemExists(Guid id)
    {
        return (_context.CourseItems?.Any(e => e.CourseItemId == id)).GetValueOrDefault();
    }
}