using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CourseAppChallenge.Areas.Content.Pages.Lectures;

[Authorize(Policy = "RequireAdministratorRole")]
public class CreateLectureModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateLectureModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public CreateLectureViewModel CreateLectureViewModel { get; set; }

    public Guid? CourseItemId { get; set; }
    
    [Display(Name = "Module")] public string CourseItemTitle { get; set; } = string.Empty;

    public async Task<IActionResult> OnGet(Guid? id)
    {
        if (id == null) return BadRequest(new ErrorResultViewModel("Id can not be null"));
        var courseItem = await _context.CourseItems.AsNoTracking().FirstOrDefaultAsync(x => x.CourseItemId == id);
        if (courseItem == null) return NotFound(new ErrorResultViewModel("Can not find this module."));
        CourseItemTitle = courseItem.CourseItemTitle;
        CourseItemId = id;
        
        
        try
        {
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

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (!ModelState.IsValid) return Page();
        if (id == null) return BadRequest(new ErrorResultViewModel("Id can not be null"));
        var courseItem = await _context.CourseItems.FirstOrDefaultAsync(x => x.CourseItemId == id);
        if (courseItem == null) return NotFound(new ErrorResultViewModel("Can not find this module."));
        
        var entry = await _context.Lectures.AddAsync(new Lecture());
        entry.CurrentValues.SetValues(CreateLectureViewModel);
        entry.Entity.CourseItem = courseItem;
        entry.Entity.CourseItemId = courseItem.CourseItemId;

        try
        {
            await _context.SaveChangesAsync();
            return RedirectToPage("./DetailsLecture", new {id = entry.Entity.LectureId});
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