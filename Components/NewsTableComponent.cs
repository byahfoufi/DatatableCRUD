using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatatableCRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatatableCRUD.Components
{
    public class NewsTableComponent : ViewComponent
    {
        private readonly EmployeeContext _context;

        public NewsTableComponent(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? SelectedCategoryId, int pageIndex = 1, int pageSize = 3)
        {
            try
            {
                var model = new NewsPageModel
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                IQueryable<News> newsQuery = _context.News.Include(n => n.Category);

                if (SelectedCategoryId.HasValue)
                {
                    newsQuery = newsQuery.Where(n => n.CategoryId == SelectedCategoryId.Value);
                }

                model.TotalItems = await newsQuery.CountAsync();
                model.TotalPages = (int)Math.Ceiling((double)model.TotalItems / pageSize);

                // Calculate how many items to skip based on the page index
                int skip = (pageIndex - 1) * pageSize;

                // Retrieve news items for the current page
                model.News = await newsQuery.Skip(skip).Take(pageSize).ToListAsync();

                
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return Content("Error: Unable to retrieve news data.");
            }
        }
    }
}
