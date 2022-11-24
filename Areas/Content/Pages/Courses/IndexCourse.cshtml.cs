using CourseAppChallenge.Data;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CourseAppChallenge.Areas.Content.Pages.Courses;

public class IndexCourseModel: PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexCourseModel(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IList<GetCourseViewModel> GetCoursesViewModel { get; set; } = new List<GetCourseViewModel>();

    public async Task<IActionResult> OnGet()
    {
        try
        {
            var courses = await _context.Courses.AsNoTracking().ToListAsync();
            if (!courses.Any()) return RedirectToPage("./CreateCourse");
            
            foreach (var course in courses)
            {
                GetCoursesViewModel.Add(course);
            }

            return Page();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ErrorResultViewModel("COIN X01 - Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("COIN X02 - Something is wrong.", ex.Message));
        }
    }
}