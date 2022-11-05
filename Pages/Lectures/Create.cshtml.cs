using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CourseAppChallenge.Data;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.LectureViewModels;
using Microsoft.EntityFrameworkCore;

namespace CourseAppChallenge.Pages.Lectures;

public class CreateModel : CourseItemNamePageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public CreateLectureViewModel CreateLectureViewModel { get; set; }

    [BindProperty] public Lecture Lecture { get; set; } = default!;

    public IActionResult OnGet()
    {
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var entry = await _context.Lectures.AddAsync(new Lecture());
        entry.CurrentValues.SetValues(CreateLectureViewModel);

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