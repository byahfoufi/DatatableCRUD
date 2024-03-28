namespace DatatableCRUD.Models
{
    public class NewsFilterModel
    {
        public int? SelectedCategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
