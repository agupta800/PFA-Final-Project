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
namespace PFA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JobPostDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<IdentityUser> userManager, JobPostDbContext context, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        public IActionResult Admin()
        {
            return View();
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
                    return RedirectToAction("newJob");
                }

                // If ModelState is not valid, return to the Create view with the validation errors
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult JobList()
        {
            return View();
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



        public async Task<IActionResult> Updatejob(JobPostModel models)
        {
            try
            {
                _context.Update(models);
                await _context.SaveChangesAsync(); // Save changes asynchronously

                return Json(new { success = true, message = "Data updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating data: {ex.Message}, Models: {Newtonsoft.Json.JsonConvert.SerializeObject(models)}");
                return Json(new { success = false, message = "Error updating data: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteJob(int id)
        {
            // Log or debug the received ID
            Console.WriteLine($"Received JobID for deletion: {id}");

            // Perform the deletion logic here
            // ...

            return Json(new { success = true, message = "Data deleted successfully" });
        }


    }
}
