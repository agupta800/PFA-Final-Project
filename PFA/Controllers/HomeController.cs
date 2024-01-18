using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFA.JobModel;
using PFA.Models;
using System.Diagnostics;

namespace PFA.Controllers
{
    //[Authorize]
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JobPostDbContext _context;

        public HomeController(ILogger<HomeController> logger, JobPostDbContext context)
        {
            _logger = logger;
            _context = context;
        }
      
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult FindaJobs()
        {

            var jobPosts = _context.JobPosts.ToList();

            return View(jobPosts);
        }

        public IActionResult About()
        {
            return View();
        }  
        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
