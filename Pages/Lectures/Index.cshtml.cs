using System.Data.Common;
using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseAppChallenge.Pages.Lectures;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public GetLecturesViewModel GetLecturesViewModel { get; set; } = default!;

    public async Task<ActionResult> OnGetAsync()
    {
        try
        {
            GetLecturesViewModel = await _context.Lectures!.AsNoTracking()
                .Include(x => x.CourseItem).ToListAsync();

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