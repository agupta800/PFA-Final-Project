using Arebis.Core.AspNet.Mvc.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PFA.Repository.Interface;
using PFA.Repository.Service;
using PFA.ViewModel;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using AspNetCoreHero.ToastNotification.Abstractions;


namespace PFA.Controllers.Authentication
{
    //[Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInmanager;
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IStringLocalizer _localizer;
        private readonly ILogger<AccountController> _logger;
        public INotyfService _notification { get; }



        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender EmailSender, IWebHostEnvironment webHostEnvironment, IStringLocalizer localizer, ILogger<AccountController> logger, INotyfService notyfService)
        {
            this.userManager = userManager;
            this.signInmanager = signInManager;
            emailSender = EmailSender;
            this.webHostEnvironment = webHostEnvironment;
            _localizer = localizer;
            _logger = logger;  // Injected ILogger instance

            _notification = notyfService;

        }
        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            int numberofUsers = userManager.Users.Count();
            ViewBag.NumberOfUsers = numberofUsers;
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
                        TempData["success"] = "Login successfull!";
                        //_notification.Success("Login sucessfully");

                        return RedirectToAction("Index", "Home");

                    }

                    if (result.IsLockedOut)
                    {
                        // Handle account lockout
                        // You can redirect to a lockout page or show a custom message
                        return View("Lockout");
                    }

                    ModelState.AddModelError(string.Empty, "Incorrect Password");
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
            //_notification.Success("Logout sucessfully");

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
                _notification.Success("User Deleted Sucessfully");
                return RedirectToAction("UserList", "Admin");
            }

            // If there are errors in the delete operation, add them to ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Delete", user);
        }



        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var existingUser = await userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                _notification.Error("User doesnot exsits");
                //TempData["error"] = "User Doesnot exist";
                return View();
            }
            var vm = new ResetPasswordVM()
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName,
                Email = existingUser.Email,

            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existingUser = await userManager.FindByIdAsync(vm.Id);
            if (existingUser == null)
            {
                _notification.Error("User doesnot exist");
                //TempData["error"] = "User Doesnot exist";

                return View(vm);
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(existingUser);
            var result = await userManager.ResetPasswordAsync(existingUser, token, vm.NewPassword);
            if (result.Succeeded)
            {
                _notification.Success("Password reset succussfully");

                return RedirectToAction("UserList", "Admin");
            }
            return View(vm);
        }

        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordUserVM forget)
        {
            ModelState.Remove("UserId");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Token");
            if (!ModelState.IsValid)
            {
                return View(forget);
            }

            var user = await userManager.FindByEmailAsync(forget.Email);
            if (user != null)
            {
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword1", "Account", new { userId = user.Id, Token = code }, protocol: Request.Scheme);

                bool isSendEmail = await emailSender.EmailSenderAsync(
            forget.Email,
             "ResetPassword",
            "Please reset your password by clicking " +
            "<a style='display:inline-block; padding:12px 24px; font-size:16px; font-weight:bold; text-align:center; text-decoration:none; background-color:#04aa6d; color:#ffffff; border-radius:5px;' href=\"" + callbackUrl + "\">Reset Password</a>"
                );

                if (isSendEmail)
                {
                    Response response = new Response();
                    response.Message = "Reset Link Send";
                    return RedirectToAction("ForgetPasswordConfirmation", response);
                }
            }
            return View();
        }

        public IActionResult ForgetPasswordConfirmation(Response response)
        {

            return View(response);
        }


        public IActionResult ResetPassword1(string userId, string Token)
        {
            var model = new ForgetPasswordUserVM
            {
                Token = Token,
                UserId = userId,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword1(ForgetPasswordUserVM forget)
        {
            ModelState.Remove("Email");

            if (!ModelState.IsValid)
            {
                return View(forget);
            }

            var user = await userManager.FindByIdAsync(forget.UserId);

            if (user == null)
            {
                return View(forget);
            }

            var result = await userManager.ResetPasswordAsync(user, forget.Token, forget.Password);

            if (result.Succeeded)
            {
                // Consider using TempData to pass messages to the next request
                _notification.Success("Reset sucessfully");

                //TempData["success"] = "Your password has been reset successfully.";
                return RedirectToAction("ForgetResetConfirmed");
            }

            // If the password reset fails, add model errors to display appropriate messages
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(forget);
        }

        public IActionResult ForgetResetConfirmed()
        {
            return View();
        }

    }

}


