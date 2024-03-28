﻿using System.ComponentModel.DataAnnotations;

namespace DatatableCRUD.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
