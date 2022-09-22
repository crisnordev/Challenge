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
        var courses = await _context.Courses.Select(x => new GetCoursesViewModel
            {
                CourseId = x.CourseId,
                CourseTitle = x.CourseTitle,
                Tag = x.Tag
            })
            .AsNoTracking().ToListAsync();

        if (!courses.Any()) return RedirectToAction(nameof(Create));

        return View(courses);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return BadRequest("Id must not be null.");

        var course = await _context.Courses.AsNoTracking().Select(x => new GetCourseByIdViewModel
            {
                CourseId = x.CourseId,
                CourseTitle = x.CourseTitle,
                Tag = x.Tag,
                Summary = x.Summary,
                Duration = x.Duration,
                CourseItems = x.CourseItems.Select(y => y.CourseItemTitle).ToList()
            })
            .FirstOrDefaultAsync(m => m.CourseId == id);
        
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

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return BadRequest("Id must not be null.");

        var model = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(x => x.CourseId == id);

        if (model == null) return RedirectToAction(nameof(Index));

        return View((EditCourseViewModel)model);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditCourseViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var course = await _context.Courses.FindAsync(id);
        
        if (course == null) return RedirectToAction(nameof(Index));
        
        course.CourseTitle = model.CourseTitle;
        course.Tag = model.Tag;
        course.Summary = model.Summary;
        course.Duration = model.Duration;

        try
        {
            _context.Courses.Update(course);
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