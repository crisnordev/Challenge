using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseAppChallenge.Areas.Content.Pages.Lectures;

[Authorize(Policy = "RequireAdministratorRole")]
public class EditLectureModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditLectureModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public EditLectureViewModel EditLectureViewModel { get; set; }

    public Guid? CourseItemId { get; set; }

    public Guid? LectureId { get; set; } 

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("LEED X 01 - Id can not be null."));
        var lecture = await _context.Lectures.AsNoTracking().FirstOrDefaultAsync(m => m.LectureId == id);
        if (lecture == null) return NotFound(new ErrorResultViewModel("LEED X 02 - Can not find this lecture."));
        LectureId = id;
        CourseItemId = lecture.CourseItemId;
        EditLectureViewModel = lecture;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("LEED X 03 - Id can not be null."));
        if (!ModelState.IsValid) return Page();
        var lecture = await _context.Lectures.FirstOrDefaultAsync(m => m.LectureId == id);
        if (lecture == null) return NotFound(new ErrorResultViewModel("LEED X 04 - Can not find this lecture."));

        try
        {
            await TryUpdateModelAsync(
                lecture!,
                "editlectureviewmodel", 
                c => c.LectureTitle, c => c.Description, c => c.VideoUrl);

            _context.Lectures.Update(lecture);
            await _context.SaveChangesAsync();

            return RedirectToPage("./DetailsLecture", new {id = lecture.LectureId});
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ErrorResultViewModel("LEED X 05 - Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("LEED X 06 - Something is wrong.", ex.Message));
        }
    }
}