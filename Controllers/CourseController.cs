using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge.Data;
using Challenge.ViewModels;
using Challenge.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Challenge.Controllers;

[Authorize]
public class CourseController : Controller
{
    private readonly ApplicationDbContext _context;

    public CourseController(ApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var databaseCourses = await _context.Courses.AsNoTracking().ToListAsync();

        if (!databaseCourses.Any()) return RedirectToAction(nameof(Create));

        var courses = databaseCourses.Select(x => (GetCoursesViewModel)x).ToList();

        return View(courses);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return BadRequest("Id must not be null.");

        var course = await _context.Courses.Include(x => x.CourseItems)
            .AsNoTracking().FirstOrDefaultAsync(m => m.CourseId == id);

        if (course == null) return RedirectToAction(nameof(Index));

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

    public async Task<IActionResult> Edit(Guid? id, EditCourseViewModel model)
    {
        if (id == null) return BadRequest("Id must not be null.");

        model = await _context.Courses.Include(x => x.CourseItems)
            .AsNoTracking().FirstOrDefaultAsync(x => x.CourseId == id);

        if (model == null) return RedirectToAction(nameof(Index));

        return View(model);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditCourseViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var course = await _context.Courses.FindAsync(id);
        course = model;

        try
        {
            _context.Update(course);
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

        var course = await _context.Courses.AsNoTracking()
            .FirstOrDefaultAsync(m => m.CourseId == id);
        
        if (course == null) return RedirectToAction(nameof(Index));

        return View(course);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var course = await _context.Courses.FindAsync(id);

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}