using GraduateWork.Data;
using GraduateWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Sockets;

namespace GraduateWork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            return View( await _dbContext.Projects.ToListAsync());
        }
        
        [HttpGet]
        public async Task<IActionResult> Board(int? Id)//Project id
        {
            //var project_obj = _dbContext.Projects.Where(p => p.Id == Id);
            //ViewBag.ProjectObj = project_obj;
            var columns_dbContext = _dbContext.ProjectColumns.Where(c => c.ProjectId == Id).Include(c => c.Issues);
            return View(await columns_dbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EndSprint(int Id)//Project id
        {
            DateTime current = DateTime.Now;
            //var sprint_context = _dbContext.Sprints.Where(s => s.ProjectId == Id).ToListAsync();
            ViewBag.ID = Id;
            var sprint_context = _dbContext.Sprints.Where(s => s.EndDate > current && s.ProjectId == Id).Include(c => c.Project).ToListAsync();
            return PartialView("EndSprint", await sprint_context);
        }

        [HttpGet]
        public async Task<IActionResult> FinishSprint(int Id)//Project id
        {
            Sprint sprint = await _dbContext.Sprints.Where(s => s.Id == Id).FirstOrDefaultAsync();
            sprint.EndDate = DateTime.Now;
            _dbContext.Update(sprint);
            await _dbContext.SaveChangesAsync();
            var project_id = await _dbContext.Projects.Where(s => s.Id == sprint.ProjectId).FirstOrDefaultAsync();
            return RedirectToAction("Board", "Home", project_id);
        }
        [HttpGet]
        public async Task<IActionResult> ManageUsers(int? Id)//Movie id
        {
            var project_id = await _dbContext.Projects.Where(p => p.Id == Id).FirstOrDefaultAsync();
            return RedirectToAction("Index","ProjectUsers", project_id);
        }

        //[HttpPost]
        //public async Task<IActionResult> ManageUsers(int? Id)//Movie id
        //{
        //    var columns_dbContext = _dbContext.ProjectColumns.Where(c => c.ProjectId == Id).Include(c => c.Issues);
        //    return View(await columns_dbContext.ToListAsync());
        //}

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Issues()
        {
            return RedirectToAction("Index", "Issues");
        }
        public IActionResult Projects()
        {
            return RedirectToAction("Index", "Projects");
        }
        public IActionResult ProjectColumns()
        {
            return RedirectToAction("Index", "ProjectColumns");
        }
        public IActionResult ProjectUsers()
        {
            return RedirectToAction("Index", "ProjectUsers");
        }

        public IActionResult Sprints()
        {
            return RedirectToAction("Index", "Sprints");
        }
    }
}