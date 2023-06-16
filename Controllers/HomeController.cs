using GraduateWork.Data;
using GraduateWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<IActionResult> Board(int? Id, int? sprintId = null)//Project id
        {
            ViewData["SelectList"] = new SelectList(_dbContext.Sprints, dataValueField:"Id", dataTextField:"Name");
            IEnumerable<Sprint> sprints = await _dbContext.Sprints.Where(s => s.ProjectId == Id).ToListAsync();
            ViewBag.Sprint = sprints;


            DateTime current = DateTime.Now;

            if (sprintId == null)
            {
                sprintId = await _dbContext.Sprints
                    .Where(s => s.ProjectId == Id && s.EndDate > current && s.StartDate < current)
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();
            }
            
            var columns = _dbContext.ProjectColumns
                .Include(c => c.Issues.Where(i => i.SprintId == sprintId))
                .Where(c => c.ProjectId == Id);
            return View(await columns.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EndSprint(int Id)
        {
            DateTime current = DateTime.Now;
            ViewBag.ID = Id;
            var sprint_context = _dbContext.Sprints.Where(s => s.EndDate > current && s.ProjectId == Id).Include(c => c.Project).ToListAsync();
            return PartialView(await sprint_context);
        }

        [HttpGet]
        public async Task<IActionResult> FinishSprint(int Id)//Sprint id
        {
            Sprint sprint = await _dbContext.Sprints.Where(s => s.Id == Id).FirstOrDefaultAsync();
            sprint.EndDate = DateTime.Now;
            _dbContext.Update(sprint);
            await _dbContext.SaveChangesAsync();
            var project_id = await _dbContext.Projects.Where(s => s.Id == sprint.ProjectId).FirstOrDefaultAsync();
            return RedirectToAction("Board", "Home", project_id);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers(int? Id)//Project id
        {
            var project_id = await _dbContext.Projects.Where(p => p.Id == Id).FirstOrDefaultAsync();
            return RedirectToAction("Index","ProjectUsers", project_id);
        }

        [HttpGet]
        public async Task<IActionResult> SprintStatistic(int? Id)
        {
            var issues_context = await _dbContext.Issues.Where(Issues => Issues.SprintId == Id).ToListAsync();
            List<decimal> estimatedTimes = new List<decimal>();
            List<decimal> elapsedTimes = new List<decimal>();

            List<String> issueNames = new List<String>();
            decimal totalEstimatedTime = 0;
            decimal totalElapsedTime = 0;

            foreach (var issue in issues_context)
            {
                decimal estimateDecimalValue = Convert.ToDecimal(issue.EstimatedTime.TotalHours);
                decimal elapsedDecimalValue = Convert.ToDecimal(issue.EllapsedTime?.TotalHours ?? 0);
                totalEstimatedTime += estimateDecimalValue;
                totalElapsedTime += elapsedDecimalValue;
                issueNames.Add(issue.Title);
                estimatedTimes.Add(estimateDecimalValue);
                elapsedTimes.Add(elapsedDecimalValue);
            }

            estimatedTimes.Add(totalEstimatedTime);
            elapsedTimes.Add(totalElapsedTime);
            issueNames.Add("TotalTime");

            ViewBag.EstimatedTimes = estimatedTimes;
            ViewBag.ElapsedTimes = elapsedTimes;
            ViewBag.IssueNames = issueNames;
            DateTime current = DateTime.Now;
            var sprint = await _dbContext.Sprints.Where(s => s.Id == Id).FirstOrDefaultAsync();
            if (sprint == null)
            {
                ViewBag.Message = "You didn't select sprint!!!";
            }
            else {
            ViewBag.Message = "This is a statistic from " + sprint.Name + " sprint! In the last column you can see total count" ;
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> projectStatistic(int? Id)
        {
            var issues_context = await _dbContext.Issues.Where(Issues => Issues.ProjectColumn.ProjectId == Id).ToListAsync();
            List<decimal> estimatedTimes = new List<decimal>();
            List<decimal> elapsedTimes = new List<decimal>();
            List<String> issueNames = new List<String>();
            decimal totalEstimatedTime = 0;
            decimal totalElapsedTime = 0;

            foreach (var issue in issues_context)
            {
                decimal estimateDecimalValue = Convert.ToDecimal(issue.EstimatedTime.TotalHours);
                decimal elapsedDecimalValue = Convert.ToDecimal(issue.EllapsedTime?.TotalHours ?? 0);
                totalEstimatedTime += estimateDecimalValue;
                totalElapsedTime += elapsedDecimalValue;
                estimatedTimes.Add(estimateDecimalValue);
                elapsedTimes.Add(elapsedDecimalValue);
                issueNames.Add(issue.Title);
            }

            estimatedTimes.Add(totalEstimatedTime);
            elapsedTimes.Add(totalElapsedTime);
            issueNames.Add("TotalTime");
            ViewBag.IssueNames = issueNames;
            ViewBag.EstimatedTimes = estimatedTimes;
            ViewBag.ElapsedTimes = elapsedTimes;
            var project = await _dbContext.Projects.Where(p => p.Id == Id).FirstOrDefaultAsync();
            ViewBag.Message = "This is a statistic from " + project.Name + " project! In the last column you can see total count";
            return View("SprintStatistic");
        }
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