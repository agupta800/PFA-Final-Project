using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFA.BlogVM;
using PFA.Data;
using PFA.JobModel;
using PFA.Models;
using System.Diagnostics;
using X.PagedList;

namespace PFA.Controllers
{
    //[Authorize]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JobPostDbContext _context;
        private readonly ApplicationContext application;
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(ILogger<HomeController> logger, JobPostDbContext context, ApplicationContext application, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            this.application = application;
            this.userManager = userManager;
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

        public async Task<IActionResult> About()
        {
            var page = await application.Pages!.FirstOrDefaultAsync(x => x.Slug == "about");
            var vm = new PageVM()
            {
                Title = page!.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };
            return View(vm);
        }
        public async Task<IActionResult> Blog(int? page)
        {
            var vm = new HomeVM();
            var setting = application.Settings!.ToList();
            vm.Title = setting[0].Title;
            vm.ShortDescription = setting[0].ShortDescription;
            vm.ThumbnailUrl = setting[0].ThumbnailUrl;
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            // Explicitly cast the result to the correct type
            vm.Posts = await application.Posts!
                .OrderByDescending(x => x.CreatedDate)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(vm);
        }







        public async Task<IActionResult> Contact()
        {

            var page = await application.Pages!.FirstOrDefaultAsync(x => x.Slug == "contact");
            PageVM vm = new PageVM()
            {
                Title = page!.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };
            return View(vm);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
