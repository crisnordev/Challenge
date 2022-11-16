using System.Data.Common;
using courseappchallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.Models;
using courseappchallenge.ViewModels;
using courseappchallenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Authorization;

namespace courseappchallenge.Pages.Lectures;

[Authorize(Policy = "RequireAdministratorRole")]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public DeleteLectureViewModel DeleteLectureViewModel { get; set; } = default!;

    [BindProperty] public Lecture Lecture { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));

        try
        {
            Lecture = await _context.Lectures.Include(y => y.CourseItem)
                .FirstOrDefaultAsync(x => x.LectureId == id);

            DeleteLectureViewModel = Lecture!;

            return Page();
        }
        catch (DbException ex)
        {
            return string.IsNullOrEmpty(Lecture?.LectureTitle)
                ? NotFound(new ErrorResultViewModel("Can not find this lecture."))
                : StatusCode(500, new ErrorResultViewModel("Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("Something is wrong.", ex.Message));
        }
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        try
        {
            _context.Lectures.Remove(Lecture);
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