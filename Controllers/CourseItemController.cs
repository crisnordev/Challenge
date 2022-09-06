using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge.Data;
using Challenge.Models;
using Challenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Challenge.Controllers
{
    public class CourseItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return await _context.CourseItems.ToListAsync() != null
                ? View(await _context.CourseItems.AsNoTracking().ToListAsync())
                : Problem("Entity set 'ApplicationDbContext.Modules'  is null.");
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || await _context.CourseItems.ToListAsync() == null)
            {
                return NotFound();
            }

            var courseItem = await _context.CourseItems.Include(x => x.Course)
                .Include(x => x.Lectures).AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseItemId == id);

            if (courseItem == null)
            {
                return NotFound();
            }

            return View(courseItem);
        }

        public IActionResult Create(CreateCourseItemViewModel model)
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseTitle");
            
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirm(CreateCourseItemViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var course = await _context.Courses.FindAsync(model.CourseId);
            var courseItem = new CourseItem(model.CourseItemTitle, model.Order, course);
            course.CourseItems.ToList().Add(courseItem);

            _context.Update(course);
            await _context.AddAsync(courseItem);
            await _context.SaveChangesAsync();
            
            View(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id, EditCourseItemViewModel model)
        {
            if (id == null || await _context.CourseItems.ToListAsync() == null)
            {
                return NotFound();
            }

            var item = await _context.CourseItems.Include(x => x.Course)
                .AsNoTracking().FirstOrDefaultAsync(x => x.CourseItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            model.CourseItemTitle = item.CourseItemTitle;
            model.Order = item.Order;
            
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirm(Guid id, EditCourseItemViewModel model)
        {
            var courseItem = await _context.CourseItems.FindAsync(id);

            if (courseItem == null) return NotFound();

            if (!ModelState.IsValid) return View(model);

            courseItem.CourseItemTitle = model.CourseItemTitle;
            courseItem.Order = model.Order;

            try
            {
                _context.Update(courseItem);
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(courseItem.CourseItemId))
                {
                    return NotFound();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || await _context.CourseItems.ToListAsync() == null)
            {
                return NotFound();
            }

            var courseItem = await _context.CourseItems.AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseItemId == id);

            if (courseItem == null)
            {
                return NotFound();
            }

            return View(courseItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await _context.CourseItems.ToListAsync() == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Modules'  is null.");
            }

            var module = await _context.CourseItems.FindAsync(id);
            if (module == null) return RedirectToAction(nameof(Index));

            _context.CourseItems.Remove(module);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(Guid id)
        {
            return (_context.CourseItems?.AsNoTracking().Any(e => e.CourseItemId == id)).GetValueOrDefault();
        }
    }
}