using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatatableCRUD.Models;

namespace DatatableCRUD.Components
{
    public class NewsFilterComponent : ViewComponent
    {
        private readonly EmployeeContext _context;

        public NewsFilterComponent(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();

                // Create a new instance of the NewsFilter model
                var filterModel = new NewsFilterModel
                {
                    Categories = categories
                };

                // Pass the filter model to the view
                return View(filterModel);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return Content("Error: Unable to retrieve filter data.");
            }
        }
    }
}
