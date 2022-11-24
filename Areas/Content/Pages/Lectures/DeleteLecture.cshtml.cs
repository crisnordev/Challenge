using System.Data.Common;
using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseAppChallenge.Areas.Content.Pages.Lectures;

[Authorize(Policy = "RequireAdministratorRole")]
public class DeleteLectureModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteLectureModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] 
    public DeleteLectureViewModel DeleteLectureViewModel { get; set; } = default!;

    public Guid? LectureId { get; set; }

    public Guid? CourseItemId { get; set; }
    
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));
        var lecture = await _context.Lectures.AsNoTracking().Include(x => x.CourseItem)
            .FirstOrDefaultAsync(x => x.LectureId == id);
        if (lecture == null) return NotFound(new ErrorResultViewModel("Can not find this lecture."));
        LectureId = id;
        CourseItemId = lecture.CourseItemId;
        DeleteLectureViewModel = lecture;

            return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));
        var lecture = await _context.Lectures.FirstOrDefaultAsync(x => x.LectureId == id);
        if (lecture == null) return NotFound(new ErrorResultViewModel("Can not find this lecture."));
        
        try
        {
            _context.Lectures.Remove(lecture);
            await _context.SaveChangesAsync();

            return RedirectToPage("../CourseItems/DetailsCourseItem", new {id = lecture.CourseItemId});
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