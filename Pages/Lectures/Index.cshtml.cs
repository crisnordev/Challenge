using System.Data.Common;
using courseappchallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.ViewModels;
using courseappchallenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Authorization;

namespace courseappchallenge.Pages.Lectures;

[Authorize(Roles = "Administrator, Student")]
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