using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge.Data;
using Challenge.Models;

namespace Challenge.Controllers
{
    public class ModuleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModuleController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Modules != null ? 
                          View(await _context.Modules.AsNoTracking().ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Modules'  is null.");
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Modules == null)
            {
                return NotFound();
            }

            var module = await _context.Modules.AsNoTracking()
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleTitle,Order")] Module model)
        {
            var module = new Module(model.ModuleTitle, model.Order);
            
            if (!ModelState.IsValid) return View(module);
            
            _context.Add(module);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Modules == null)
            {
                return NotFound();
            }

            var module = await _context.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }
            
            return View(module);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ModuleTitle,Order")] Module module)
        {
            if (id != module.ModuleId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(module);
            
            try
            {
                _context.Update(module);
                await _context.SaveChangesAsync();
            }
            
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(module.ModuleId))
                {
                    return NotFound();
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Modules == null)
            {
                return NotFound();
            }

            var module = await _context.Modules
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Modules == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Modules'  is null.");
            }
            
            var module = await _context.Modules.FindAsync(id);
            if (module == null) return RedirectToAction(nameof(Index));
            
            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(Guid id)
        {
          return (_context.Modules?.Any(e => e.ModuleId == id)).GetValueOrDefault();
        }
    }
}
