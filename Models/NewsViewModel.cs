using Microsoft.AspNetCore.Mvc.Rendering;

namespace DatatableCRUD.Models
{
    public class NewsViewModel
    {
        public News News { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }

}
