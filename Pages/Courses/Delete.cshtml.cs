using courseappchallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.Models;
using courseappchallenge.ViewModels.CourseViewModels;

namespace courseappchallenge.Pages.Courses;

    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Course Course { get; set; } = default!;
        
        [BindProperty] public DeleteCourseByIdViewModel DeleteCourseByIdViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Courses == null) return NotFound();

            var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == id);

            if (course == null) return NotFound();

            DeleteCourseByIdViewModel = course;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Courses == null) return NotFound();
            
            var course = await _context.Courses.FindAsync(id);

            if (course == null) return RedirectToPage("./Index");
            
            Course = course;
            _context.Courses.Remove(Course);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
