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

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectColumns.Include(p => p.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> CreateOrEdit(int? id = null)
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id");
            IEnumerable<Project> projects = _context.Projects;
            ViewBag.Project = projects;
            ProjectColumn? projectColumn = null;
            if (id == null)
            {
                projectColumn = new ProjectColumn();
            }
            else
            {
                projectColumn = await _context.ProjectColumns.FindAsync(id);
                if (projectColumn == null)
                {
                    return NotFound();
                }
            }
            return PartialView("CreateOrEdit", projectColumn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(ProjectColumn projectColumn)
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", projectColumn.ProjectId);
            IEnumerable<Project> projects = _context.Projects;
            ViewBag.Project = projects;
            if (projectColumn.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(projectColumn);

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
                    _context.Update(projectColumn);
                }
                else
                {
                    return BadRequest("Not valid");
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

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

            return PartialView(projectColumn);
        }

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

            return PartialView(projectColumn);
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
    }
}
