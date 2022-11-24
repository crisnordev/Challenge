using CourseAppChallenge.Data;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CourseAppChallenge.Areas.Content.Pages.Courses;

[Authorize(Policy = "RequireAdministratorRole")]
public class CreateCourseModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateCourseModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] 
    public CreateCourseViewModel CreateCourseViewModel { get; set; } = default!;
    
    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var entry = await _context.Courses.AddAsync(new Course());
        entry.CurrentValues.SetValues(CreateCourseViewModel);
        
        try
        {
            await _context.SaveChangesAsync();
                
            return RedirectToPage("./DetailsCourse", new {id = entry.Entity.CourseId});
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