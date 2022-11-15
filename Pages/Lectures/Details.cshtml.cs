using System.Data.Common;
using courseappchallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.ViewModels;
using courseappchallenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Authorization;

namespace courseappchallenge.Pages.Lectures;

[Authorize(Policy = "RequireAdministratorRole")]
[Authorize(Policy = "RequireStudentRole")]
public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public GetLectureByIdViewModel GetLectureByIdViewModel { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));

        try
        {
            GetLectureByIdViewModel = await _context.Lectures.AsNoTracking().Include(y => y.CourseItem)
                .FirstOrDefaultAsync(x => x.LectureId == id);

            return Page();
        }
        catch (DbException ex)
        {
            return string.IsNullOrEmpty(GetLectureByIdViewModel?.LectureTitle)
                ? NotFound(new ErrorResultViewModel("Can not find this lecture."))
                : StatusCode(500, new ErrorResultViewModel("Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("Something is wrong.", ex.Message));
        }
    }
}