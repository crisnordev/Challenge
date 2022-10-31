using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using courseappchallenge.Data;
using courseappchallenge.Models;
using courseappchallenge.ViewModels.CourseItemViewModels;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace courseappchallenge.Pages.CourseItems;

public class CreateModel : CourseNamePageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public CourseItem CourseItem { get; set; } = default!;

    [BindProperty] public CreateCourseItemViewModel CreateCourseItemViewModel { get; set; } = default!;

    public IActionResult OnGet()
    {
        PopulateCoursesDropDownList(_context);
        
        return Page();
    }
    
    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.CourseItems == null || CourseItem == null) return Page();
        
        var emptyCourseItem = new CourseItem();

        try
        {
            await TryUpdateModelAsync(
                emptyCourseItem,
                "CreateCourseItemViewModel", // Prefix for form value.
                s => s.CourseItemTitle, s => s.Order, s => s.CourseId);

            await _context.CourseItems.AddAsync(emptyCourseItem);
            
            await _context.SaveChangesAsync();
                
            return RedirectToPage("./Index");
        }
        catch (DbUpdateException)
        {
            PopulateCoursesDropDownList(_context, emptyCourseItem.Course);
            return Page();
        }
    }
}

