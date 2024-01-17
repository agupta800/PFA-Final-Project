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

namespace PFA.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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
                var role = roles.FirstOrDefault() ??"Register User";

                var userViewModel = new UserViewModel
                {
                    User = user,
                    RoleName = role
                };

                userViewModelList.Add(userViewModel);
            }

            return View(userViewModelList);
        }
        public IActionResult JobList()
        {
            return View();
        }


    }
}
