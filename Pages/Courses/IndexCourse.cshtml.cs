using System.Data.Common;
using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseAppChallenge.Pages.Courses;

[AllowAnonymous]
public class IndexCourseModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexCourseModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<GetCourseViewModel> ListGetCourseViewModel { get; set; } = new List<GetCourseViewModel>();

    public async Task<ActionResult> OnGetAsync()
    {
        var courses = await _context.Courses!.AsNoTracking().ToListAsync();
        
        try
        {
            foreach (var item in courses)
            {
                ListGetCourseViewModel.Add(item);
            } 

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