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

        public AdminController(UserManager<IdentityUser> userManager, JobPostDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
    }
}
