
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFA.BlogModel;
using PFA.BlogVM;
using PFA.Data;
using X.PagedList;



namespace PFA.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> userManager, _userManager;

        public PostController(ApplicationContext context,
                                INotyfService notyfService,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<IdentityUser> userManager)
        {
            _context = context;
            _notification = notyfService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
    [HttpGet]
        //public async Task<IActionResult> Index(int? page)
        //{
        //    var listOfPosts = new List<Post>();

        //    var loggedInUser = await _userManager.FindByNameAsync(User.Identity!.Name);

        //    if (loggedInUser != null)
        //    {
        //        listOfPosts = await _context.Posts
        //            .Where(x => x.ApplicationUserId == loggedInUser.Id)
        //            .ToListAsync();

        //        var listOfPostsVM = listOfPosts.Select(x => new PostVM()
        //        {
        //            Id = x.Id,
        //            Title = x.Title,
        //            CreatedDate = x.CreatedDate,
        //            ThumbnailUrl = x.ThumbnailUrl,
        //            AuthorName = _userManager.GetUserNameAsync(loggedInUser).Result

        //        }).ToList();

        //        int pageSize = 5;
        //        int pageNumber = page ?? 1;

        //        return View(await listOfPostsVM.OrderByDescending(x => x.CreatedDate).ToPagedListAsync(pageNumber, pageSize));
        //    }

        //    return NotFound();
        //}

        public async Task<IActionResult> Index(int? page)
        {
            var listOfPosts = await _context.Posts
                .ToListAsync();

            var listOfPostsVM = listOfPosts.Select(x => new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ThumbnailUrl = x.ThumbnailUrl,
                //AuthorName = _userManager.GetUserNameAsync(loggedInUser).Result
            }).ToList();

            int pageSize = 5;
            int pageNumber = page ?? 1;

            return View(await listOfPostsVM.OrderByDescending(x => x.CreatedDate).ToPagedListAsync(pageNumber, pageSize));
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            if (!ModelState.IsValid) 
            {
                return View(vm);
            }

            //get logged in user id
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            var post = new Post();

            post.Title = vm.Title;
            post.Description = vm.Description;
            post.ShortDescription = vm.ShortDescription;
            post.ApplicationUserId = loggedInUser!.Id;

            if (post.Title != null)
            {
                string slug = vm.Title!.Trim();
                slug = slug.Replace(" ", "-");
                post.Slug = slug + "-" + Guid.NewGuid();
            }

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.Posts!.AddAsync(post);
            await _context.SaveChangesAsync();
            _notification.Success("Post Created Successfully");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);

            var loggedInUser = await _userManager.FindByNameAsync(User.Identity?.Name);

            if (loggedInUser != null && (loggedInUser.Id == post?.ApplicationUserId))
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                _notification.Success("Post Deleted Successfully");
                return RedirectToAction("Index", "Post");
            }

            // Handle unauthorized deletion or post not found
            _notification.Error("You are not authorized or post not found");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                _notification.Error("Post not found");
                return View();
            }

            var loggedInUser = await _userManager.FindByNameAsync(User.Identity?.Name);

            if (loggedInUser != null && loggedInUser.Id == post.ApplicationUserId)
            {
                var vm = new CreatePostVM
                {
                    Id = post.Id,
                    Title = post.Title,
                    ShortDescription = post.ShortDescription,
                    Description = post.Description,
                    ThumbnailUrl = post.ThumbnailUrl,
                };

                return View(vm);
            }

            // Handle unauthorized edit or post not found
            _notification.Error("You are not Authorized");
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CreatePostVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (post == null)
            {
                _notification.Error("Post not found");
                return View();
            }

            post.Title = vm.Title;
            post.ShortDescription = vm.ShortDescription;
            post.Description = vm.Description;

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("Post updated succesfully");
            return RedirectToAction("Index", "Post");
        }


        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }

    }
}
