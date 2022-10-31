using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using courseappchallenge.Data;
using courseappchallenge.Models;
using courseappchallenge.ViewModels;

namespace courseappchallenge.Pages.CourseItems;

    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public CourseItem CourseItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.CourseItems == null)
            {
                return NotFound();
            }

            var courseitem = await _context.CourseItems.FirstOrDefaultAsync(m => m.CourseItemId == id);

            if (courseitem == null)
            {
                return NotFound();
            }
            else 
            {
                CourseItem = courseitem;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.CourseItems == null)
            {
                return NotFound();
            }
            var courseitem = await _context.CourseItems.FindAsync(id);

            if (courseitem != null)
            {
                CourseItem = courseitem;
                _context.CourseItems.Remove(CourseItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
