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

    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CourseItem> CourseItem { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.CourseItems != null)
            {
                CourseItem = await _context.CourseItems.ToListAsync();
            }
        }
    }
