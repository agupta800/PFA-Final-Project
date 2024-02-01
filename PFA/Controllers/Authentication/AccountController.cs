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


namespace PFA.Controllers.Authentication
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInmanager;
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IStringLocalizer _localizer;
        private readonly ILogger<AccountController> _logger;




        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender EmailSender, IWebHostEnvironment webHostEnvironment, IStringLocalizer localizer, ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInmanager = signInManager;
            emailSender = EmailSender;
            this.webHostEnvironment = webHostEnvironment;
            _localizer = localizer;
            _logger = logger;  // Injected ILogger instance



        }
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
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]


        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(model.Email);

                    if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return View("ForgotPasswordConfirmation");
                    }

                    // Generate a password reset token
                    string code = await userManager.GeneratePasswordResetTokenAsync(user);

                    // Construct the password reset callback URL
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);

                    // Send the password reset email using IEmailSender
                    var emailBody = $"Please reset your password by clicking <a href=\"{HtmlEncoder.Default.Encode(callbackUrl)}\">here</a>";
                    await emailSender.EmailSenderAsync(model.Email, "Reset Your Password", emailBody);

                    _logger.LogInformation("Password reset email sent successfully.");

                    return RedirectToAction("ForgotPasswordConfirmation");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the ForgotPassword method.");
                // Log the exception and handle it appropriately (e.g., show a generic error page)
                return View("Error");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


    }
}

