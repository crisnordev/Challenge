using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseAppChallenge.Pages.Courses.CourseItems.Lectures;

[Authorize(Policy = "RequireAdministratorRole")]
public class EditLectureModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditLectureModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public EditLectureViewModel EditLectureViewModel { get; set; }

    public Guid? LectureId { get; set; } 

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));
        LectureId = id;

        var lecture = await _context.Lectures.AsNoTracking().FirstOrDefaultAsync(m => m.LectureId == id);
        if (lecture == null) return NotFound(new ErrorResultViewModel("Can not find this lecture."));

        EditLectureViewModel = lecture;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (!ModelState.IsValid) return Page();
        
        var lecture = await _context.Lectures.FirstOrDefaultAsync(m => m.LectureId == id);
        if (lecture == null) return NotFound(new ErrorResultViewModel("Can not find this lecture."));

        try
        {
            await TryUpdateModelAsync(
                lecture!,
                "editlectureviewmodel", 
                c => c.LectureTitle, c => c.Description, c => c.VideoUrl);

            _context.Lectures.Update(lecture);
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