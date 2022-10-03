using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.Data;
using courseappchallenge.ViewModels;
using courseappchallenge.ViewModels.CourseItemViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace courseappchallenge.Controllers;

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
        var courseItem = await _context.CourseItems.Include(x =>
            x.Course).Select(x => new GetCourseItemsViewModel
        {
            CourseItemId = x.CourseItemId,
            CourseItemTitle = x.CourseItemTitle,
            Order = x.Order,
            CourseTitle = x.Course.CourseTitle
        }).AsNoTracking().OrderBy(x => x.CourseTitle)
            .ThenBy(x => x.Order).ToListAsync();
            
        if (!courseItem.Any()) return RedirectToAction(nameof(Create));
            
        return View(courseItem);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return BadRequest("Id must not be null.");

        var courseItem = await _context.CourseItems.Include(x => x.Course)
            .Include(x => x.Lectures).AsNoTracking()
            .Select(x => new GetCourseItemByIdViewModel
            {
                CourseItemId = x.CourseItemId,
                CourseItemTitle = x.CourseItemTitle,
                Order = x.Order,
                CourseId = x.Course.CourseId,
                CourseTitle = x.Course.CourseTitle,
                Lectures = x.Lectures.Select(x => x.LectureTitle).ToList()
            }).FirstOrDefaultAsync(m => m.CourseItemId == id);

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
        if (!ModelState.IsValid) return RedirectToAction(nameof(Create));

        var course = _context.Courses.Include(x => x.CourseItems)
            .FirstOrDefault(y => y.CourseId == model.CourseId);
            
        var existingItemInOrder = course.CourseItems.FirstOrDefault(x => x.Order == model.Order);
            
        if (existingItemInOrder != null)
        {
            model.ExistingOrder = true;
            model.Order = 0;
            return RedirectToAction(nameof(Create), new {model});
        }

        var courseItem = (CourseItem)model;

        if (course != null)
        {
            courseItem.Course = course;
            course.CourseItems.ToList().Add(courseItem);
            _context.Update(course);
        }
        
        await _context.AddAsync(courseItem);
        await _context.SaveChangesAsync();

        View(model);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid? id, EditCourseItemViewModel model)
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
        if (!ModelState.IsValid) return RedirectToAction(nameof(Edit));
        
        var courseItem = await _context.CourseItems.Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.CourseItemId == id);
        
        var existingItemInOrder = _context.CourseItems.AsNoTracking()
            .Where(x => x.Course.CourseId == courseItem.Course.CourseId)
            .FirstOrDefaultAsync(x => x.Order == model.Order).Result;
        
        if (existingItemInOrder != null)
        {
            model.ExistingOrder = true;
            model.CourseId = existingItemInOrder.Course.CourseId;
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