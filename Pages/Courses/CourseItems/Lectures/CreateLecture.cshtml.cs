using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CourseAppChallenge.Pages.Courses.CourseItems.Lectures;

[Authorize(Policy = "RequireAdministratorRole")]
public class CreateLectureModel : CourseItemNamePageModel
{
    private readonly ApplicationDbContext _context;

    public CreateLectureModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] 
    public CreateLectureViewModel CreateLectureViewModel { get; set; }

    public Guid? CourseItemId { get; set; }

    public async Task<IActionResult> OnGet(Guid? courseItemId)
    {
        if (courseItemId != null)
        {
            CourseItemId = courseItemId;
            CreateLectureViewModel.CourseItem = await _context.CourseItems.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CourseItemId == courseItemId);

            return Page();
        }
        try
        {
            PopulateCourseItemsDropDownList(_context);

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

    public async Task<IActionResult> OnPostAsync(Guid? courseItemId)
    {
        if (!ModelState.IsValid) return Page();
        
        if (courseItemId != null)
        {
            CourseItemId = courseItemId;
            CreateLectureViewModel.CourseItem = await _context.CourseItems
                .FirstOrDefaultAsync(x => x.CourseItemId == courseItemId);
        }
        else
        {
            CreateLectureViewModel.CourseItem = await _context.CourseItems
                .FirstOrDefaultAsync(x => x.CourseItemId == CourseItemId);
        }

        var entry = await _context.Lectures.AddAsync(new Lecture());
        entry.CurrentValues.SetValues(CreateLectureViewModel);
        entry.CurrentValues.SetValues(entry.Entity.CourseItem == CreateLectureViewModel.CourseItem);

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