using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFA.Models;
using System.Diagnostics;

namespace PFA.Controllers
{
    //[Authorize]
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
      
        public IActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        public IActionResult FindaJobs()
        {
            return View();
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
