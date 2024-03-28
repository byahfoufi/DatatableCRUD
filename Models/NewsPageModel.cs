namespace DatatableCRUD.Models
{
    public class NewsPageModel
    {
        public List<News> News { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

    }
}
