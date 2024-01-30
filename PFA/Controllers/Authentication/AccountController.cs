using Arebis.Core.AspNet.Mvc.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PFA.Repository.Interface;
using PFA.Repository.Service;
using PFA.ViewModel;

namespace PFA.Controllers.Authentication
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInmanager;
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IStringLocalizer _localizer;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender EmailSender, IWebHostEnvironment webHostEnvironment, IStringLocalizer localizer)
        {
            this.userManager = userManager;
            this.signInmanager = signInManager;
            emailSender = EmailSender;
            this.webHostEnvironment = webHostEnvironment;
            _localizer = localizer;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            try
            {
                if (ModelState.IsValid)


                {
                    var chkEmail = await userManager.FindByEmailAsync(model.Email);
                    if (chkEmail != null)
                    {
                        ModelState.AddModelError(string.Empty, "Email already exist");
                        return View();
                    }
                    var user = new IdentityUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,

                    };
                    var result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        TempData["success"] = "User SuccessFully Register";

                        string path = Path.Combine(webHostEnvironment.WebRootPath, "Template/Template.cshtml");
                        string htmlString = System.IO.File.ReadAllText(path);
                        htmlString = htmlString.Replace("{{title}}", "User Registration");
                        htmlString = htmlString.Replace("{{Username}}", model.UserName);
                        htmlString = htmlString.Replace("{{Email}}", model.Email);
                        htmlString = htmlString.Replace("{{Password}}", model.Password);
                        bool status = await emailSender.EmailSenderAsync(
                         model.Email,
                         "Account Successfully Created",
                          htmlString
                          );
                        await signInmanager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");

                    }
                    if (result.Errors.Count() > 0)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }

      
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(model.Email);

                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Email not found");
                        return View(model);
                    }

                    // Check the password
                    var result = await signInmanager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        TempData["success"] = "Login successful!";

                        return RedirectToAction("Index", "Home");
                    }

                    if (result.IsLockedOut)
                    {
                        // Handle account lockout
                        // You can redirect to a lockout page or show a custom message
                        return View("Lockout");
                    }

                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                // logger.LogError(ex, "Exception during user login");
                throw;
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInmanager.SignOutAsync();
            TempData["success"] = "You have been successfully logged out.";

            return RedirectToAction("Login", "Account");
        }


        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["success"] = "User deleted successfully!";
                return RedirectToAction("Index", "Home");
            }

            // If there are errors in the delete operation, add them to ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Delete", user);
        }

    }
}

