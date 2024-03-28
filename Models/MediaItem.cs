using System.ComponentModel.DataAnnotations;

namespace DatatableCRUD.Models
{
    public class MediaItem
    {
        
        public int MediaItemId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Link { get; set; } = "";

        [Required]
        public MediaType MediaType { get; set; }

        public string FileName { get; set; } = "";
    }
}


public enum MediaType
    {
        YouTube,
        Image,
        File
    }

