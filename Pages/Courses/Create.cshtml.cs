using courseappchallenge.Data;
using courseappchallenge.Models;
using courseappchallenge.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace courseappchallenge.Pages.Courses;

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


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Courses == null || Course == null) return Page();

        var emptyCourse = new Course();
        
        try
        {
            await TryUpdateModelAsync(
                emptyCourse,
                "CreateCourseViewModel", // Prefix for form value.
                s => s.CourseTitle, s => s.Tag, s => s.Summary, s => s.Duration);

            await _context.Courses.AddAsync(emptyCourse);
            
            await _context.SaveChangesAsync();
                
            return RedirectToPage("./Index");
        }
        catch (DbUpdateException)
        {
            return Page();
        }
    }
}