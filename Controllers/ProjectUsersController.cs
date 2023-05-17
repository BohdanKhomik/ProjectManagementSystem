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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectUsers.Include(p => p.Project).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectUsers/Details/5
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

            return View(projectUser);
        }

        // GET: ProjectUsers/Create
        public IActionResult Create()
        {
            var users = _context.ApplicationUsers.ToList();
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id");
            ViewData["UserId"] = new SelectList(users, "Id", "Id");
            return View();
        }

        // POST: ProjectUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserId,ProjectId")] ProjectUser projectUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", projectUser.ProjectId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", projectUser.UserId);
            return View(projectUser);
        }

        // GET: ProjectUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProjectUsers == null)
            {
                return NotFound();
            }

            var projectUser = await _context.ProjectUsers.FindAsync(id);
            if (projectUser == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", projectUser.ProjectId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", projectUser.UserId);
            return View(projectUser);
        }

        // POST: ProjectUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserId,ProjectId")] ProjectUser projectUser)
        {
            if (id != projectUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectUserExists(projectUser.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", projectUser.ProjectId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", projectUser.UserId);
            return View(projectUser);
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

            return View(projectUser);
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

        private bool ProjectUserExists(int id)
        {
          return (_context.ProjectUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
