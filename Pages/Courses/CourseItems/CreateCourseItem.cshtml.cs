using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CourseAppChallenge.Pages.Courses.CourseItems;

[Authorize(Policy = "RequireAdministratorRole")]
public class CreateCourseItemModel : CourseNamePageModel
{
    private readonly ApplicationDbContext _context;

    public CreateCourseItemModel(ApplicationDbContext context)
    {
        _context = context;
    }


    [BindProperty] 
    public CreateCourseItemViewModel CreateCourseItemViewModel { get; set; } = default!;
    
    public Guid? CourseId { get; set; }


    public async Task<IActionResult> OnGet(Guid? courseId)
    {
        if (courseId != null)
        {
            CourseId = courseId;
            CreateCourseItemViewModel.Course =
                await _context.Courses.AsNoTracking().FirstOrDefaultAsync(x => x.CourseId == courseId);
        }
        
        try
        {
            PopulateCoursesDropDownList(_context);

            return Page();
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

    public async Task<IActionResult> OnPostAsync(Guid? courseId)
    {
        if (!ModelState.IsValid) return Page();

        if (courseId != null)
        {
            CourseId = courseId;
            CreateCourseItemViewModel.Course = await _context.Courses
                .FirstOrDefaultAsync(x => x.CourseId == courseId);
        }
        else
        {
            CreateCourseItemViewModel.Course = await _context.Courses
                .FirstOrDefaultAsync(x => x.CourseId == CourseId);
        }

        var entry = await _context.CourseItems.AddAsync(new CourseItem());
        entry.CurrentValues.SetValues(CreateCourseItemViewModel);
        entry.CurrentValues.SetValues(entry.Entity.Course == CreateCourseItemViewModel.Course);

        try
        {
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