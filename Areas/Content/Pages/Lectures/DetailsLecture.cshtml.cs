using System.Data.Common;
using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseAppChallenge.Areas.Content.Pages.Lectures;

[Authorize]
public class DetailsLectureModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsLectureModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public GetLectureByIdViewModel GetLectureByIdViewModel { get; set; } = default!;
    
    public Guid? LectureId { get; set; } = Guid.Empty;

    public Guid? CourseItemId { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));
        var lecture = await _context.Lectures.AsNoTracking()
            .Include(y => y.CourseItem)
            .FirstOrDefaultAsync(x => x.LectureId == id);
        if (lecture == null) return NotFound(new ErrorResultViewModel("Can not find lecture."));
        GetLectureByIdViewModel = lecture;
        LectureId = id;
        CourseItemId = lecture.CourseItemId;

        try
        {
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