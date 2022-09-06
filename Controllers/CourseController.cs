using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge.Data;
using Challenge.Models;
using Challenge.ViewModels.CourseViewModels;

namespace Challenge.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return await _context.Courses.ToListAsync() != null ? 
                          View(await _context.Courses.AsNoTracking().ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || await _context.Courses.ToListAsync() == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.Include(x => x.CourseItems).AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseId == id);
            
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var course = new Course(model.CourseTitle, model.Tag, model.Summary, model.Duration);
            
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            View(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(x => x.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            var editCourse = new EditCourseViewModel(course.CourseTitle, course.Tag, course.Summary, course.Duration);

            return View(editCourse);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditCourseViewModel model)
        {
            var course = await _context.Courses.FindAsync(id);
            
            if (course == null) return NotFound();

            if (!ModelState.IsValid) return View(model);
            
            course.CourseTitle = model.CourseTitle;
            course.Tag = model.Tag;
            course.Summary = model.Summary;
            course.Duration = model.Duration;

            try
            {
                _context.Update(course);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || await _context.Courses.ToListAsync() == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await _context.Courses.ToListAsync() == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
            }
            
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return RedirectToAction(nameof(Index));
            
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(Guid id)
        {
          return (_context.Courses?.AsNoTracking().Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}
