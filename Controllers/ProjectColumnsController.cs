using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GraduateWork.Data;
using GraduateWork.Models;

namespace GraduateWork.Controllers
{
    public class ProjectColumnsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectColumnsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectColumns
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectColumns.Include(p => p.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectColumns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProjectColumns == null)
            {
                return NotFound();
            }

            var projectColumn = await _context.ProjectColumns
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectColumn == null)
            {
                return NotFound();
            }

            return View(projectColumn);
        }

        // GET: ProjectColumns/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id");
            return View();
        }

        // POST: ProjectColumns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ProjectId")] ProjectColumn projectColumn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectColumn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", projectColumn.ProjectId);
            return View(projectColumn);
        }

        // GET: ProjectColumns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProjectColumns == null)
            {
                return NotFound();
            }

            var projectColumn = await _context.ProjectColumns.FindAsync(id);
            if (projectColumn == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", projectColumn.ProjectId);
            return View(projectColumn);
        }

        // POST: ProjectColumns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ProjectId")] ProjectColumn projectColumn)
        {
            if (id != projectColumn.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectColumn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectColumnExists(projectColumn.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", projectColumn.ProjectId);
            return View(projectColumn);
        }

        // GET: ProjectColumns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProjectColumns == null)
            {
                return NotFound();
            }

            var projectColumn = await _context.ProjectColumns
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectColumn == null)
            {
                return NotFound();
            }

            return View(projectColumn);
        }

        // POST: ProjectColumns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProjectColumns == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProjectColumns'  is null.");
            }
            var projectColumn = await _context.ProjectColumns.FindAsync(id);
            if (projectColumn != null)
            {
                _context.ProjectColumns.Remove(projectColumn);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectColumnExists(int id)
        {
          return (_context.ProjectColumns?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
