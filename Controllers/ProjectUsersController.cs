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
    public class ProjectUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectUsers
        public async Task<IActionResult> Index(int? Id)
        {
            var applicationDbContext = _context.ProjectUsers.Where(p => p.ProjectId == Id).Include(p => p.Project).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectUsers/Create
        public async Task<IActionResult> CreateOrEdit(int? id = null)
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id");
            IEnumerable<Project> projects = _context.Projects;
            ViewBag.Project = projects;

            var users = _context.ApplicationUsers.ToList();
            ViewData["UserId"] = new SelectList(users, "Id", "Id");
            ViewBag.Users = users;
            ProjectUser? projectUser = null;
            if (id == null)
            {
                projectUser = new ProjectUser();
            }
            else
            {
                projectUser = await _context.ProjectUsers.FindAsync(id);
                if (projectUser == null)
                {
                    return NotFound();
                }
            }
            return PartialView("CreateOrEditProjectUser", projectUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(ProjectUser projectUser)
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", projectUser.ProjectId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", projectUser.UserId);
            if (projectUser.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(projectUser);

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
                    _context.Update(projectUser);
                }
                else
                {
                    return BadRequest("Not valid");
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { Id = projectUser.ProjectId });
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProjectUsers == null)
            {
                return NotFound();
            }

            var projectUser = await _context.ProjectUsers
                .Include(p => p.Project)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectUser == null)
            {
                return NotFound();
            }

            return PartialView("Details", projectUser);
        }
        // GET: ProjectUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProjectUsers == null)
            {
                return NotFound();
            }

            var projectUser = await _context.ProjectUsers
                .Include(p => p.Project)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectUser == null)
            {
                return NotFound();
            }

            return PartialView("Delete", projectUser);
        }

        // POST: ProjectUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProjectUsers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProjectUsers'  is null.");
            }
            var projectUser = await _context.ProjectUsers.FindAsync(id);
            if (projectUser != null)
            {
                _context.ProjectUsers.Remove(projectUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
