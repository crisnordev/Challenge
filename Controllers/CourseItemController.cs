using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge.Data;
using Challenge.ViewModels;
using Challenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Challenge.Controllers;

[Authorize]
public class CourseItemController : Controller
{
    private readonly ApplicationDbContext _context;

    public CourseItemController(ApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var databaseCourseItem = await _context.CourseItems.Include(x =>
            x.Course).AsNoTracking().OrderBy(x => x.Order).ToListAsync();
            
        if (!databaseCourseItem.Any()) return RedirectToAction(nameof(Create));

        var courseItem = databaseCourseItem.Select(x => 
            (GetCourseItemsViewModel)x).ToList();
            
        return View(courseItem);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return BadRequest("Id must not be null.");

        var courseItem = await _context.CourseItems.Include(x => x.Course)
            .Include(x => x.Lectures).AsNoTracking()
            .FirstOrDefaultAsync(m => m.CourseItemId == id);

        if (courseItem == null) return RedirectToAction(nameof(Index));

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

        var course = await _context.Courses.Include(x => x.CourseItems)
            .FirstOrDefaultAsync(y => y.CourseId == model.CourseId);
            
        var item = course.CourseItems.FirstOrDefault(x => x.Order == model.Order);
            
        if (item != null)
        {
            model.ExistingOrder = true;
            model.ExistingOrderItemId = item.CourseItemId;
            model.Order = 0;
            return RedirectToAction(nameof(Create), new {model});
        }

        var courseItem = (CourseItem)model;
        course.CourseItems.ToList().Add(courseItem);

        _context.Update(course);
        await _context.AddAsync(courseItem);
        await _context.SaveChangesAsync();

        View(model);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id, EditCourseItemViewModel model)
    {
        if (id == null) return BadRequest("Id must not be null.");

        model = await _context.CourseItems.Include(x => x.Course)
            .AsNoTracking().FirstOrDefaultAsync(x => x.CourseItemId == id);
            
        if (model == null) return RedirectToAction(nameof(Index));

        return View(model);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditConfirm(Guid id, EditCourseItemViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        
        var courseItem = await _context.CourseItems.Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.CourseItemId == id);

        var item = _context.CourseItems.AsNoTracking()
            .Where(x => x.Course.CourseId == courseItem.Course.CourseId)
            .FirstOrDefaultAsync(x => x.Order == model.Order).Result;
        
        if (item != null)
        {
            model.ExistingOrder = true;
            model.CourseId = item.Course.CourseId;
            model.Order = 0;
            return RedirectToAction(nameof(Edit), new {id, model});
        }
            
        courseItem = model;

        try
        {
            _context.Update(courseItem);
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

        var courseItem = await _context.CourseItems.AsNoTracking()
            .FirstOrDefaultAsync(m => m.CourseItemId == id);

        if (courseItem == null) return RedirectToAction(nameof(Index));

        return View(courseItem);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var module = await _context.CourseItems.FindAsync(id);
        
        _context.CourseItems.Remove(module);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}