using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.Data;
using courseappchallenge.ViewModels;
using courseappchallenge.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace courseappchallenge.Controllers
{
    [Authorize]
    public class LectureController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LectureController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var lectures = await _context.Lectures.Include(x => x.CourseItem)
                .Select(x => new GetLecturesViewModel
                {
                    LectureId = x.LectureId,
                    LectureTitle = x.LectureTitle,
                    CourseItemTitle = x.CourseItem.CourseItemTitle
                })
                .AsNoTracking().ToListAsync();

            if (!lectures.Any()) return RedirectToAction(nameof(Create));

            return View(lectures);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return BadRequest("Id must not be null.");

            var lecture = await _context.Lectures.Include(x => x.CourseItem)
                .Select(x => new GetLectureByIdViewModel
                {
                    LectureId = x.LectureId,
                    LectureTitle = x.LectureTitle,
                    Description = x.Description,
                    VideoUrl = x.VideoUrl,
                    CourseItemId = x.CourseItem.CourseItemId,
                    CourseItemTitle = x.CourseItem.CourseItemTitle
                })
                .AsNoTracking().FirstOrDefaultAsync(m => m.LectureId == id);
            
            if (lecture == null) return RedirectToAction(nameof(Index));

            return View(lecture);
        }

        public IActionResult Create(CreateLectureViewModel model)
        {
            ViewData["CourseItemId"] = new SelectList(_context.CourseItems, "CourseItemId", "CourseItemTitle");

            return View(model);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirm(CreateLectureViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var courseItem = _context.CourseItems.FirstOrDefault(x => x.CourseItemId == model.CourseItemId);
            
            var lecture = (Lecture)model;

            if (courseItem != null)
            {
                lecture.CourseItem = courseItem;
                courseItem.Lectures.ToList().Add(lecture);
                _context.Update(courseItem);
            }
            
            await _context.Lectures.AddAsync(lecture);
            await _context.SaveChangesAsync();
            
            View(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id, EditLectureViewModel? model)
        {
            if (id == null) return BadRequest("Id must not be null.");

            model = await _context.Lectures.AsNoTracking().FirstOrDefaultAsync(x => x.LectureId == id);
            
            if (model == null) return RedirectToAction(nameof(Index));
            
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirm(Guid id, EditLectureViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var lecture = await _context.Lectures.FindAsync(id);

            lecture = model;

            try
            {
                _context.Update(lecture);
                await _context.SaveChangesAsync();
            }
            catch (DbException)
            {
                StatusCode(500, "Internal server error.");
            }
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest("Id must not be null.");

            var lecture = await _context.Lectures.AsNoTracking().FirstOrDefaultAsync(m => m.LectureId == id);
            
            if (lecture == null) return RedirectToAction(nameof(Index));

            return View(lecture);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var lecture = await _context.Lectures.FindAsync(id);

            _context.Lectures.Remove(lecture);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
