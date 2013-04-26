using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BookmarkManager.Model
{
    public class Bookmark
    {
        public Bookmark()
        {
            BookmarkTags = new HashSet<Tag>();
        }

        // Primitive properties

        [Key] 
        public int BookmarkId { get; set; }
        [Required]
        public string BookmarkUrl { get; set; }
        public string BookmarkTitle { get; set; }
        public string BookmarkDescription { get; set; }
        public int BookmarkRating { get; set; }
        public int? BookmarkPosition { get; set; }

        // Navigation properties
        public ICollection<Tag> BookmarkTags { get; set; }
    }
}
