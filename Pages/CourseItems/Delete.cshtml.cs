using System.Data.Common;
using courseappchallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.Models;
using courseappchallenge.ViewModels;
using courseappchallenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;

namespace courseappchallenge.Pages.CourseItems;

[Authorize(Roles = "Administrator")]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public CourseItem CourseItem { get; set; }

    [BindProperty] public DeleteCourseItemViewModel DeleteCourseItemViewModel { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));

        try
        {
            CourseItem = await _context.CourseItems.Include(y => y.Course)
                .FirstOrDefaultAsync(x => x.CourseItemId == id);

            DeleteCourseItemViewModel = CourseItem!;

            return Page();
        }
        catch (DbException ex)
        {
            return string.IsNullOrEmpty(CourseItem?.CourseItemTitle)
                ? NotFound(new ErrorResultViewModel("Can not find this module."))
                : StatusCode(500, new ErrorResultViewModel("Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("Something is wrong.", ex.Message));
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            _context.CourseItems.Remove(CourseItem);
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
}