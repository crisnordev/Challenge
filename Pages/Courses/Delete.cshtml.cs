using System.Data.Common;
using CourseAppChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using CourseAppChallenge.ViewModels.CourseViewModels;

namespace CourseAppChallenge.Pages.Courses;

    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Course Course { get; set; } = default!;
        
        [BindProperty] public DeleteCourseViewModel DeleteCourseViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null) return NotFound(new ErrorResultViewModel("Id can not be null."));
            
            try
            {
                Course = await _context.Courses.Include(y => y.CourseItems)
                    .FirstOrDefaultAsync(x => x.CourseId == id);

                DeleteCourseViewModel = Course!;

                return Page();
            }
            catch (DbException ex)
            {
                return string.IsNullOrEmpty(Course?.CourseTitle)
                    ? NotFound(new ErrorResultViewModel("Can not find this course."))
                    : StatusCode(500, new ErrorResultViewModel("Internal server error.", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResultViewModel("Something is wrong.", ex.Message));
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            try
            {
                _context.Courses.Remove(Course);
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
