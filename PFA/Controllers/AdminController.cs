using Arebis.Core.AspNet.Mvc.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using PFA.Data;
using PFA.Models;
using PFA.Repository.Interface;
using PFA.Repository.Service;
using PFA.ViewModel;
using PFA.JobModel;
using System;
using AspNetCoreHero.ToastNotification.Abstractions;
namespace PFA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JobPostDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public INotyfService _notification { get; }

        public AdminController(UserManager<IdentityUser> userManager, JobPostDbContext context, ILogger<AdminController> logger, INotyfService notyfService)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _notification = notyfService;
        }
        public IActionResult Admin()
        {
            //return View("~/Views/Account/Index.cshtml", ViewBag.NumberOfUsers);
            return RedirectToAction("Index", "Account");
        }


        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();

            var userViewModelList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "Register User";

                var userViewModel = new UserViewModel
                {
                    User = user,
                    RoleName = role
                };

                userViewModelList.Add(userViewModel);
            }

            return View(userViewModelList);
        }
        [HttpGet]
        public IActionResult newJob()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult newJob(JobPostModel model)
        {
            {
                if (ModelState.IsValid)
                {
                    _context.JobPosts.Add(model);
                    _context.SaveChanges();
                    TempData["success"] = "Job added successfully.";

                    return RedirectToAction("FindaJobs", "Home");
                }

                // If ModelState is not valid, return to the Create view with the validation errors
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult JobList(JobPostModel model)
        {
            _context.JobPosts.ToList();
            return View(model);
        }
        public async Task<IActionResult> AllJobs()
        {
            var allData = await _context.JobPosts.ToListAsync();
            return Json(allData);
            
        }

        [HttpGet]
        public IActionResult Updatejob()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Updatejob(JobPostModel models)
        {
            try
            {
                _context.Update(models);
                await _context.SaveChangesAsync(); // Save changes asynchronously
                _notification.Success("Data Updated Sucessfully");

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating data: {ex.Message}, Models: {Newtonsoft.Json.JsonConvert.SerializeObject(models)}");
                return Json(new { success = false, message = "Error updating data: " + ex.Message });
            }
        }

        public IActionResult DeleteJob(int JobId)
        {
            try
            {
                var jobToDelete = _context.JobPosts.Find(JobId);
                if (jobToDelete != null)
                {
                    _context.JobPosts.Remove(jobToDelete);
                    _context.SaveChanges();

                    _notification.Success("Job deleted Sucessfully");
                    return Json(new { success = true });

                }
                else
                {
                    _notification.Error("Job not found");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the job: " + ex.Message;
            }

            return RedirectToAction("Index"); // Redirect to a relevant action, such as the action that renders your view
        }


        [HttpGet]
        public async Task<IActionResult> GetJobCount()
        {
            var count = await _context.JobPosts.CountAsync();
            return Json(count);
        }


    }
}
