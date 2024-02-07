﻿
//using Microsoft.AspNetCore.Identity;
//using PFA.Blog_Model;
//using PFA.Blog_ModelPFA;
//using PFA.Data;
//using PFA.Utilites;


//namespace FineBlog.Utilites
//{
//    public class DbInitializer : IDbInitializer
//    {
//        private readonly ApplicationContext _context;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        public DbInitializer(ApplicationContext context,
//                               UserManager<ApplicationUser> userManager,
//                               RoleManager<IdentityRole> roleManager)
//        {
//            _context = context;
//            _userManager = userManager;
//            _roleManager = roleManager;
//        }

//        public void Initialize()
//        {
//            if (!_roleManager.RoleExistsAsync(WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult())
//            {
//                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAdmin)).GetAwaiter().GetResult();
//                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAuthor)).GetAwaiter().GetResult();
//                _userManager.CreateAsync(new ApplicationUser()
//                {
//                    UserName = "admin@gmail.com",
//                    Email = "admin@gmail.com",
//                    FirstName ="Super",
//                    LastName = "Admin"
//                },"Rohit45@#").Wait();

//                var appUser = _context.ApplicationUsers!.FirstOrDefault(x => x.Email == "admin@gmail.com");
//                if (appUser != null)
//                {
//                    _userManager.AddToRoleAsync(appUser, WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult();
//                }


//                var listOfPages = new List<Page>()
//                {
//                    new Page()
//                    {
//                         Title = "About Us",
//                        Slug = "about"
//                    },
//                    new Page()
//                    {
//                        Title = "Contact Us",
//                        Slug = "contact"
//                    },
//                    new Page()
//                    {
//                        Title = "Privacy Policy",
//                        Slug = "privacy"
//                    }
//                 };

//                _context.Pages!.AddRange(listOfPages);

//                var setting = new Setting{
//                    SiteName = "Site Name",
//                    Title = "Site Title",
//                    ShortDescription = "Short Description of site"
//                };
                
//                _context.Settings!.Add(setting);
//                _context.SaveChanges();

//            }
//        }
//    }
//}
