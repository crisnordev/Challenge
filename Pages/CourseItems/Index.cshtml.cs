using System.Data.Common;
using courseappchallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.ViewModels;
using courseappchallenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;

namespace courseappchallenge.Pages.CourseItems;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public GetCourseItemsViewModel GetCourseItemsViewModel { get; set; } = default!;

    public async Task<ActionResult> OnGetAsync()
    {
        try
        {
            GetCourseItemsViewModel = await _context.CourseItems!.AsNoTracking().OrderBy(y => y.Order)
                .Include(x => x.Course).ToListAsync();

            return Page();
        }
        catch (DbException ex)
        {
            return StatusCode(500, new ErrorResultViewModel("Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("Something is wrong.", ex.Message));
        }
    }
}