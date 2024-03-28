using DatatableCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace DatatableCRUD.Controllers
{
    public class NewsController : Controller
    {
        private readonly EmployeeContext _context;

        public NewsController(EmployeeContext context)
        {
            _context = context;
        }
        // GET: News (Listing) 
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult GetNewsTable(NewsFilterModel filter ,int pageIndex = 1, int pageSize = 3)
        {
            if (filter == null)
            {
                filter = new NewsFilterModel();
            }

            //int? SelectedCategoryId, int pageIndex = 1, int pageSize = 10


            return ViewComponent("NewsTableComponent", new { SelectedCategoryId = filter.SelectedCategoryId , pageIndex = pageIndex, pageSize = pageSize });
        }


        public IActionResult GetNewsFilter()
        {
            return ViewComponent("NewsFilterComponent");
        }






        // GET: News/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            // Add similar logic for MediaItems if applicable
            return View();
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News news)
        {
            if (ModelState.IsValid)
            {
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // ... repopulate ViewData for dropdowns on validation failure ...
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            Console.WriteLine(news.ToString());
            var categories = _context.Categories.ToList();

            var list = new SelectList(categories, "CategoryId", "Name", news.CategoryId);
            ViewBag.CategoriesList = list;
            //ViewData["MediaItemId"] = new SelectList(await _context.MediaItems.ToListAsync(), "Id", "Name", news.MediaItemId); // Adjust "Name" if needed

            return View(news);
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News news, string Tags)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newsToUpdate = await _context.News
                                             .Include(n => n.NewsTags)
                                             .FirstOrDefaultAsync(n => n.Id == id);

                    if (newsToUpdate == null) return NotFound();

                    newsToUpdate.Title = news.Title;
                    newsToUpdate.Description = news.Description;
                    newsToUpdate.CategoryId = news.CategoryId;
                    newsToUpdate.MediaItemId = news.MediaItemId;

                    ProcessTags(newsToUpdate, Tags); // Same ProcessTags method from previous examples

                    _context.Update(newsToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // ... handle concurrency errors ...
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", news.CategoryId);
            ViewData["MediaItemId"] = new SelectList(await _context.MediaItems.ToListAsync(), "Id", "Name", news.MediaItemId);
            return View(news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Category) // Include related data if needed
                .FirstOrDefaultAsync(m => m.Id == id);

            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null) // Ensure news item exists 
            {
                _context.News.Remove(news);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }

        // Helper method to Process Tags
        private void ProcessTags(News news, string tags)
        {
            if (string.IsNullOrWhiteSpace(tags)) return; // Return if no tags were entered

            // Clear existing tags
            news.NewsTags.Clear();

            var tagNames = tags.Split(',').Select(t => t.Trim());

            foreach (var tagName in tagNames)
            {
                var tag = _context.Tags.FirstOrDefault(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _context.Tags.Add(tag);
                }
                news.NewsTags.Add(new NewsTag { NewsId = news.Id, TagId = tag.Id });
            }
        }
    }
}
 