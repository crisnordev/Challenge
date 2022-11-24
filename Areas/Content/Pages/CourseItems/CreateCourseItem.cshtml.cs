using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CourseAppChallenge.Areas.Content.Pages.CourseItems;

[Authorize(Policy = "RequireAdministratorRole")]
public class CreateCourseItemModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateCourseItemModel(ApplicationDbContext context)
    {
        _context = context;
    }


    [BindProperty] public CreateCourseItemViewModel CreateCourseItemViewModel { get; set; } = default!;
    
    [Display(Name = "Course")] public string CourseTitle { get; set; } = string.Empty;

    public Guid? CourseId { get; set; }


    public async Task<IActionResult> OnGet(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("CCI X01 - Id can not be null.")); 
        var course = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(x => x.CourseId == id);
        if (course == null) return NotFound(new ErrorResultViewModel("CCI X02 - Can not find this course."));
        CourseTitle = course.CourseTitle;
        CourseId = id;
        
        try
        {
            return Page();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CCI X03 - Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CCI X04 - Something is wrong.", ex.Message));
        }
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null) return NotFound(new ErrorResultViewModel("CCI X05 - Id can not be null.")); 
        if (!ModelState.IsValid) return Page();
        var course = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == id);
        if (course == null) return NotFound(new ErrorResultViewModel("CCI X06 - Can not find this course."));

        var entry = await _context.CourseItems.AddAsync(new CourseItem());
        entry.CurrentValues.SetValues(CreateCourseItemViewModel);
        entry.Entity.Course = course;
        entry.Entity.CourseId = course.CourseId;
        
        try
        {
            await _context.SaveChangesAsync();
            return RedirectToPage("./DetailsCourseItem", new {id = entry.Entity.CourseItemId});
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CCI X07 - Internal server error.", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResultViewModel("CCI X08 - Something is wrong.", ex.Message));
        }
    }
}