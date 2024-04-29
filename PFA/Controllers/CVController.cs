using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using PFA.JobModel;
using PFA.Models;
using System.Diagnostics;

namespace PFA.Controllers
{
    public class CVController : Controller
    {
        private readonly JobPostDbContext _context;
        private readonly INotyfService notification;

        public CVController(JobPostDbContext context , INotyfService notyfService)
        {
            _context = context;
            notification = notyfService;
        }
        public IActionResult Index()
        {
            var result = _context.Files.ToList();
            return View(result);
            
        }


        public IActionResult Upload()
        {
            return View();
        }



        [HttpPost]
        [Authorize] // This attribute ensures that only authenticated users can access this action
        public IActionResult Upload(FileModel model, IFormFile image)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                TempData["error"] = "Please login to upload your CV";
                return RedirectToAction("Login", "Account"); // Redirect to the Login page
            }

            // Save the image to the server
            if (image != null && image.Length > 0)
            {
                // Sanitize the file name by replacing spaces with underscores
                var fileName = Path.GetFileNameWithoutExtension(image.FileName);
                var fileExtension = Path.GetExtension(image.FileName);
                var sanitizedFileName = $"{fileName.Replace(" ", "_")}_{DateTime.Now.Ticks}{fileExtension}";

                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sanitizedFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                // Set the Image property of the model to the correct image path
                model.Image = "/images/" + sanitizedFileName;
            }

            // Generate a tracking ID with 4 digits
            Random random = new Random();
            model.TrackingId = random.Next(1000, 9999).ToString();

            // Add the model to the context and save changes
            _context.Files.Add(model);
            _context.SaveChanges();

            // Set success message
            TempData["success"] = "CV uploaded successfully. Here is your tracking ID: " + model.TrackingId;

            // Redirect to Index action with the tracking ID as a route parameter
            return RedirectToAction("NewTrack", new { trackingId = model.TrackingId });
        }




        public IActionResult NewTrack(string trackingId)
        {
            // Retrieve the file model based on the tracking ID
            var model = _context.Files.SingleOrDefault(f => f.TrackingId == trackingId);

            if (model == null)
            {
                // Handle the case where the model is not found
                return NotFound();
            }

            // Pass the model to the view
            return View(model);
        }

        [HttpGet("Download/{id}")]
        public IActionResult Download(int id)
        {
            var fileModel = _context.Files.Find(id);

            if (fileModel != null)
            {
                // Determine content type based on file extension
                string contentType = GetContentType(fileModel.Image);

                if (contentType != null)
                {
                    // Return the file for download
                    return File(fileModel.Image, contentType, fileModel.Image);
                }
            }

            return NotFound();
        }

        [HttpGet("View/{id}")]
        public IActionResult View(int id)
        {
            var fileModel = _context.Files.Find(id);

            if (fileModel != null)
            {
                // Determine content type based on file extension
                string contentType = GetContentType(fileModel.Image);

                if (contentType != null)
                {
                    // Return the file for viewing in the browser
                    return File(fileModel.Image, contentType);
                }
            }

            return NotFound();
        }

        // Helper method to determine content type based on file extension
        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (provider.TryGetContentType(fileName, out var contentType))
            {
                return contentType;
            }

            return "application/octet-stream"; // If content type cannot be determined
        }



        [HttpPost]
        public IActionResult Approve(int id)
        {
            var model = _context.Files.Find(id);

            if (model != null)
            {
                model.IsApproved = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { trackingId = model.TrackingId });
        }

        [HttpPost]
        public IActionResult Disapprove(int id)
        {
            var model = _context.Files.Find(id);

            if (model != null)
            {
                model.IsApproved = false; // Update IsApproved to false for disapproval
                model.IsDisApproved = true; // Set IsDisApproved to true for disapproval
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { trackingId = model.TrackingId });
        }



        public IActionResult Track()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetStatus(string trackingId)
        {
            // Retrieve the file model based on the tracking ID
            var model = _context.Files.SingleOrDefault(f => f.TrackingId == trackingId);

            if (model == null)
            {
                // Handle the case where the model is not found
                //ViewBag.ErrorMessage = "File not found. Please check the tracking ID.";
                TempData["Error"] = "File not found. Please check the tracking ID.";
                return RedirectToAction("Index", "Home");

            }

            // Pass the model to the view
            return View("FileStatus", model);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            {
                //get the staff with the Id
                var staff = _context.Files.FirstOrDefault(e => e.Id == Id);
                //remove the staff from the database
                _context.Files.Remove(staff);
                _context.SaveChanges();
                notification.Success("CV Deleted Successfully");

                return RedirectToAction("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}






//[HttpPost]
//public IActionResult Upload(IFormFile file)
//{
//    if (file != null && file.Length > 0)
//    {
//        using (var memoryStream = new MemoryStream())
//        {
//            file.CopyTo(memoryStream);

//            var newFile = new FileModel
//            {
//                FileName = file.FileName,
//                FileContent = memoryStream.ToArray()
//            };

//            _context.Files.Add(newFile);
//            _context.SaveChanges();
//        }
//    }

//    return RedirectToAction("Index"); // Redirect to file list or any other page
//}

//[HttpPost]
//public IActionResult Upload([FromForm] FileModel model, IFormFile file)
//{
//    if (ModelState.IsValid)
//    {
//        if (file != null && file.Length > 0)
//        {
//            using (var memoryStream = new MemoryStream())
//            {
//                file.CopyTo(memoryStream);

//                // Set the file properties in the model
//                //model.FileName = file.FileName;
//                model.FileContent = memoryStream.ToArray();

//                _context.Files.Add(model);
//                _context.SaveChanges();

//                return RedirectToAction("Index");
//            }
//        }

//        // Handle other properties of YourViewModel
//        // Save, update, or process the data as needed

//        return RedirectToAction("Success"); // Redirect to a success page or take appropriate action
//    }

//    // If ModelState is not valid, return to the form view with validation errors
//    return View("Index", model);
//}
