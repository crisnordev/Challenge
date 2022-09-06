using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge.Data;
using Challenge.Models;
using Challenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Challenge.Controllers
{
    public class LectureController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LectureController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Lectures != null ? 
                          View(await _context.Lectures.AsNoTracking().ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Lectures'  is null.");
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Lectures == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures.AsNoTracking()
                .FirstOrDefaultAsync(m => m.LectureId == id);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        
        public IActionResult Create(CreateLectureViewModel model)
        {
            ViewData["CourseItemId"] = new SelectList(_context.CourseItems, "CourseItemId", "CourseItemTitle");

            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirm(CreateLectureViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var courseItem = await _context.CourseItems.FindAsync(model.CourseItemId);
            var lecture = new Lecture(model.LectureTitle, model.Description, model.VideoUrl, courseItem);

            _context.Update(courseItem);
            await _context.Lectures.AddAsync(lecture);
            await _context.SaveChangesAsync();
            View(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Lectures == null)
            {
                return NotFound();
            }

            var item = await _context.Lectures.AsNoTracking().FirstOrDefaultAsync(x => x.LectureId == id);
            if (item == null)
            {
                return NotFound();
            }

            var lecture = new EditLectureViewModel(item.LectureTitle, item.Description, item.VideoUrl);
            
            return View(lecture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditLectureViewModel model)
        {
            var lecture = await _context.Lectures.FindAsync(id);
            
            if (lecture == null) return NotFound();

            if (!ModelState.IsValid) return View(model);

            lecture.LectureTitle = model.LectureTitle;
            lecture.Description = model.Description;
            lecture.VideoUrl = model.VideoUrl;

            try
            {
                _context.Update(lecture);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LectureExists(lecture.LectureId))
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || await _context.Lectures.ToListAsync() == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures.AsNoTracking().FirstOrDefaultAsync(m => m.LectureId == id);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Lectures == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Lectures'  is null.");
            }
            
            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture == null) return RedirectToAction(nameof(Index));
            
            _context.Lectures.Remove(lecture);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool LectureExists(Guid id)
        {
          return (_context.Lectures?.AsNoTracking().Any(e => e.LectureId == id)).GetValueOrDefault();
        }
    }
}
