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

namespace courseappchallenge.Pages.Lectures;

    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Lecture> Lecture { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Lectures != null)
            {
                Lecture = await _context.Lectures.ToListAsync();
            }
        }
    }

