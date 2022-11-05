using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.Data;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.LectureViewModels;

namespace CourseAppChallenge.Pages.Lectures;

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