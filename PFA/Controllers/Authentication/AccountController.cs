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
                    IdentityUser checkEmail = await userManager.FindByEmailAsync(model.Email);
                    if (checkEmail == null)
                    {
                        ModelState.AddModelError(string.Empty, "Email not found");
                        return View(model);
                    }
                    if (await userManager.CheckPasswordAsync(checkEmail, model.Password) == false)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Credentials");
                        return View(model);
                    }
                    var result = await signInmanager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }
            }

            catch (Exception ex)
            {
                throw;
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInmanager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}

