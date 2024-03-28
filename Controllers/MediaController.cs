using DatatableCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.RegularExpressions;
using Winista.Mime;
using FileTypeChecker;
using FileTypeChecker.Abstracts;

namespace DatatableCRUD.Controllers
{
    public class MediaController : Controller
    {
        private readonly EmployeeContext _context;

        public MediaController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: Media
        public async Task<IActionResult> Index()
        {
            var mediaItems = await _context.MediaItems.ToListAsync();

            foreach (var item in mediaItems)
            {
                item.Link = item.Link ?? "Link Unavailable";
                item.FileName = item.FileName ?? "File Unavailable";

                if (item.Link == "Link Unavailable" || item.FileName == "File Unavailable")
                {
                    // Add logging here, e.g., using a logging library or simple Console.WriteLine
                    Console.WriteLine($"MediaItem (ID: {item.MediaItemId}) has a null Link or FileName");
                }
            }


            return View(mediaItems);
        }


        // GET: Media/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Media/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( MediaItem mediaItem, IFormFile file)
        {
            Console.WriteLine("----- DEBUGGING START ------");
            Console.WriteLine($"Title: {mediaItem.Title}");
            Console.WriteLine($"Link: {mediaItem.Link}"); // If applicable
            Console.WriteLine($"MediaType: {mediaItem.MediaType}");
            Console.WriteLine($"File (is null): {file == null}"); // Check if a file was sent 

            // Perform manual validation
            bool isValid = true;
            List<string> validationErrors = new List<string>();

            // 1. Validate core properties
            if (string.IsNullOrWhiteSpace(mediaItem.Title))
            {
                isValid = false;
                validationErrors.Add("Title is required.");
            }

            // 2. Media Type specific validation
            if (mediaItem.MediaType == MediaType.YouTube)
            {
                try
                {
                    mediaItem.Link = ValidateYouTubeLink(mediaItem.Link);
                }
                catch (ArgumentException ex)
                {
                    isValid = false;
                    validationErrors.Add(ex.Message);
                }
            }
            else if (mediaItem.MediaType != MediaType.YouTube && file == null)  // File Upload
            {
                isValid = false;
                validationErrors.Add("A file is required for non-YouTube media.");
            }

            // 3. If valid, proceed with saving
            if (isValid)
            {
                if (file != null)
                {

                    mediaItem.FileName = await SaveUploadedFile(file);
                    mediaItem.Link = "";
                }



                _context.Add(mediaItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Log or display errors 
                Console.WriteLine("Validation Errors:");
                foreach (var error in validationErrors)
                {
                    Console.WriteLine(error);
                }
                // Return view with errors (consider passing errors to the view)
                return View(mediaItem);
            }
        }



        private async Task<string> SaveUploadedFile(IFormFile file)
        {
            string uniqueFileName = null;

            if (file != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                uniqueFileName =file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            Console.WriteLine("unique file name:"+uniqueFileName);
            return uniqueFileName;
        }

        private string ValidateYouTubeLink(string link)
        {
            // YouTube Video ID extraction (You can customize the pattern if needed)
            var regexPattern = @"^(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))((\w|-){11})(?:\S+)?$";
            var match = Regex.Match(link, regexPattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return "https://www.youtube.com/embed/" + match.Groups[1].Value;
            }
            else
            {
                throw new ArgumentException("Invalid YouTube Link Format");

            }
        }
    }
}
