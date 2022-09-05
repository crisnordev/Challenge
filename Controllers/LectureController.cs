using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge.Data;
using Challenge.Models;
using Challenge.ViewModels.LectureViewModels;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLectureViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var lecture = new Lecture(model.LectureTitle, model.Description, model.VideoUrl);

            _context.Add(lecture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Lectures == null)
            {
                return NotFound();
            }

            EditLectureViewModel lecture = await _context.Lectures.AsNoTracking().FirstOrDefaultAsync(
                x => x.LectureId == id);
            if (lecture == null)
            {
                return NotFound();
            }
            return View(lecture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditLectureViewModel model)
        {
            var lecture = _context.Lectures.FirstOrDefaultAsync(x => x.LectureId == id).Result;
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
