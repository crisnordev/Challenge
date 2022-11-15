using courseappchallenge.Data;
using courseappchallenge.Models;
using courseappchallenge.ViewModels;
using courseappchallenge.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace courseappchallenge.Pages.Courses;

[Authorize(Policy = "RequireAdministratorRole")]
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet() => Page();
    

    public Course Course { get; set; } = default!;
    
    [BindProperty] public CreateCourseViewModel CreateCourseViewModel { get; set; } = default!;

    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        
        var entry = await _context.Courses.AddAsync(new Course());
        entry.CurrentValues.SetValues(CreateCourseViewModel);
        
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