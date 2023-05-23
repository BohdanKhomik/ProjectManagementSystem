using GraduateWork.Data;
using GraduateWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        public async Task<IActionResult> Board(int? Id)//Movie id
        {
            var columns_dbContext = _dbContext.ProjectColumns.Where(c => c.ProjectId == Id).Include(c => c.Issues);
            return View(await columns_dbContext.ToListAsync());
        }
        [HttpGet]
        public IActionResult StartSprint()
        {
            return RedirectToAction("Create", "Sprints");
        }
        //[HttpGet]
        //public async Task<IActionResult> ManageUsers()//Movie id
        //{
        //    return View();
        //}

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}