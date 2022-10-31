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

    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
