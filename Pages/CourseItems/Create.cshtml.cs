using courseappchallenge.Data;
using Microsoft.AspNetCore.Mvc;
using courseappchallenge.Models;
using courseappchallenge.ViewModels;
using courseappchallenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace courseappchallenge.Pages.CourseItems;

[Authorize(Policy = "RequireAdministratorRole")]
public class CreateModel : CourseNamePageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }


    [BindProperty] public CreateCourseItemViewModel CreateCourseItemViewModel { get; set; } = default!;

    [BindProperty] public CourseItem CourseItem { get; set; } = default!;

    public IActionResult OnGet()
    {
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var entry = await _context.CourseItems.AddAsync(new CourseItem());
        entry.CurrentValues.SetValues(CreateCourseItemViewModel);

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