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