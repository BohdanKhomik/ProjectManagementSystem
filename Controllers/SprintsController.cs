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
    public class SprintsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SprintsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sprints
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sprints.Include(s => s.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sprints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sprints == null)
            {
                return NotFound();
            }

            var sprint = await _context.Sprints
                .Include(s => s.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sprint == null)
            {
                return NotFound();
            }

            return View(sprint);
        }

        // GET: Sprints/Create
        public async Task<IActionResult> CreateOrEdit(int? id = null)
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id");
            Sprint? sprint = null;
            if (id == null)
            {
                sprint = new Sprint();
            }
            else
            {
                sprint = await _context.Sprints.FindAsync(id);
                if (sprint == null)
                {
                    return NotFound();
                }
            }
            return PartialView("CreateOrEditSprint", sprint);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(Sprint sprint)
        {
            ViewData["AssigneeUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["ColumnId"] = new SelectList(_context.ProjectColumns, "Id", "Id");
            ViewData["ReporterUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["SprintId"] = new SelectList(_context.Sprints, "Id", "Id");
            if (sprint.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(sprint);

                }
                else
                {
                    return BadRequest("Not valid");
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Update(sprint);
                }
                else
                {
                    return BadRequest("Not valid");
                }
            }
            await _context.SaveChangesAsync();
            var project_id = await _context.Projects.Where(item => item.Id == sprint.ProjectId).FirstOrDefaultAsync();
            return RedirectToAction("Board", "Home", project_id);
        }

        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id");
            return View();
        }

        // POST: Sprints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,ProjectId")] Sprint sprint)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sprint);
                await _context.SaveChangesAsync();
                return RedirectToAction("Board", "Home");
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", sprint.ProjectId);
            return View(sprint);
        }

        // GET: Sprints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sprints == null)
            {
                return NotFound();
            }

            var sprint = await _context.Sprints.FindAsync(id);
            if (sprint == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", sprint.ProjectId);
            return View(sprint);
        }

        // POST: Sprints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,ProjectId")] Sprint sprint)
        {
            if (id != sprint.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sprint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SprintExists(sprint.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", sprint.ProjectId);
            return View(sprint);
        }

        // GET: Sprints/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sprints == null)
            {
                return NotFound();
            }

            var sprint = await _context.Sprints
                .Include(s => s.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sprint == null)
            {
                return NotFound();
            }

            return View(sprint);
        }

        // POST: Sprints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sprints == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sprints'  is null.");
            }
            var sprint = await _context.Sprints.FindAsync(id);
            if (sprint != null)
            {
                _context.Sprints.Remove(sprint);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SprintExists(int id)
        {
          return (_context.Sprints?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
