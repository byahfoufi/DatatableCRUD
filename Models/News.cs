using System.ComponentModel.DataAnnotations;

namespace DatatableCRUD.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime? PublishedDate { get; set; }

        public int? MediaItemId { get; set; }
        public MediaItem? MediaItem { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
