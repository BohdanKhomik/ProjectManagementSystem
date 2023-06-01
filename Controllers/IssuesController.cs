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
    public class IssuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IssuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Issues
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Issues.Include(i => i.Assignee).Include(i => i.ProjectColumn).Include(i => i.Reporter).Include(i => i.Sprint);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Issues == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues
                .Include(i => i.Assignee)
                .Include(i => i.ProjectColumn)
                .Include(i => i.Reporter)
                .Include(i => i.Sprint)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // GET: Issues/Create
        public async Task<IActionResult> CreateOrEdit(int? id = null)
        {
            var users = _context.ApplicationUsers.ToList();
            //ViewData["UserId"] = new SelectList(users, "Id", "Id");
            ViewBag.Users = users;
            //ViewData["ColumnId"] = new SelectList(_context.ProjectColumns, "Id", "Id");
            IEnumerable<ProjectColumn> projectColumn = _context.ProjectColumns;
            ViewBag.ProjectColumns = projectColumn;

            //ViewData["SprintId"] = new SelectList(_context.Sprints, "Id", "Id");
            IEnumerable<Sprint> sprint = _context.Sprints;
            ViewBag.Sprints = sprint;
            Issue? issue = null;
            if (id == null)
            {
                issue = new Issue();
            }
            else
            {
                issue = await _context.Issues.FindAsync(id);
                if (issue == null)
                {
                    return NotFound();
                }
            }
            return PartialView("CreateOrEditIssue", issue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(Issue issue)
        {
            ViewData["AssigneeUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["ColumnId"] = new SelectList(_context.ProjectColumns, "Id", "Id");
            ViewData["ReporterUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["SprintId"] = new SelectList(_context.Sprints, "Id", "Id");
            if (issue.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(issue);

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
                    _context.Update(issue);
                }
                else
                {
                    return BadRequest("Not valid");
                }
            }
            await _context.SaveChangesAsync();
            var issue_column = await _context.ProjectColumns.Where(item => item.Id == issue.ColumnId).FirstOrDefaultAsync();
            var project_id = await _context.Projects.Where(item => item.Id == issue_column.ProjectId).FirstOrDefaultAsync();
            //var projectId = await _context.Projects.Where(item => item.ProjectColumns.Contains(issue.ProjectColumn)).FirstOrDefaultAsync();

            return RedirectToAction("Board", "Home", project_id);
        }
        //public IActionResult Create()
        //{
        //    ViewData["AssigneeUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
        //    ViewData["ColumnId"] = new SelectList(_context.ProjectColumns, "Id", "Id");
        //    ViewData["ReporterUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
        //    ViewData["SprintId"] = new SelectList(_context.Sprints, "Id", "Id");
        //    return View();
        //}

        //// POST: Issues/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Description,ColumnId,SprintId,EstimatedTime,EllapsedTime,CreatedDate,AssigneeUserId,ReporterUserId")] Issue issue)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(issue);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AssigneeUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", issue.AssigneeUserId);
        //    ViewData["ColumnId"] = new SelectList(_context.ProjectColumns, "Id", "Id", issue.ColumnId);
        //    ViewData["ReporterUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", issue.ReporterUserId);
        //    ViewData["SprintId"] = new SelectList(_context.Sprints, "Id", "Id", issue.SprintId);
        //    return View(issue);
        //}

        //// GET: Issues/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Issues == null)
        //    {
        //        return NotFound();
        //    }

        //    var issue = await _context.Issues.FindAsync(id);
        //    if (issue == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["AssigneeUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", issue.AssigneeUserId);
        //    ViewData["ColumnId"] = new SelectList(_context.ProjectColumns, "Id", "Id", issue.ColumnId);
        //    ViewData["ReporterUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", issue.ReporterUserId);
        //    ViewData["SprintId"] = new SelectList(_context.Sprints, "Id", "Id", issue.SprintId);
        //    return View(issue);
        //}

        //// POST: Issues/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ColumnId,SprintId,EstimatedTime,EllapsedTime,CreatedDate,AssigneeUserId,ReporterUserId")] Issue issue)
        //{
        //    if (id != issue.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(issue);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!IssueExists(issue.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AssigneeUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", issue.AssigneeUserId);
        //    ViewData["ColumnId"] = new SelectList(_context.ProjectColumns, "Id", "Id", issue.ColumnId);
        //    ViewData["ReporterUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", issue.ReporterUserId);
        //    ViewData["SprintId"] = new SelectList(_context.Sprints, "Id", "Id", issue.SprintId);
        //    return View(issue);
        //}

        // GET: Issues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Issues == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues
                .Include(i => i.Assignee)
                .Include(i => i.ProjectColumn)
                .Include(i => i.Reporter)
                .Include(i => i.Sprint)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Issues == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Issues'  is null.");
            }
            var issue = await _context.Issues.FindAsync(id);
            if (issue != null)
            {
                _context.Issues.Remove(issue);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueExists(int id)
        {
          return (_context.Issues?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
