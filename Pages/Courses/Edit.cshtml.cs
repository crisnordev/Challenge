using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.Data;
using courseappchallenge.Models;
using courseappchallenge.ViewModels.CourseViewModels;

namespace courseappchallenge.Pages.Courses;

    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Course Course { get; set; } = default!;
        
        [BindProperty] public EditCourseViewModel EditCourseViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Courses == null) return NotFound();

            var course =  await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null) return NotFound();
            
            EditCourseViewModel = course;
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Courses == null) return NotFound();
            
            var course = await _context.Courses.FindAsync(id);

            if (course == null) return RedirectToPage("./Index");

            _context.Attach(Course).State = EntityState.Modified;

            try
            {
                await TryUpdateModelAsync(course, "EditCourseViewModel", i => 
                        i.CourseTitle, i => i.Tag, i => i.Summary, i => i.Duration);
                
                await _context.SaveChangesAsync();
                
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(Course.CourseId)) return NotFound();

                throw;
            }
        }

        private bool CourseExists(Guid id)
        {
          return (_context.Courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }

