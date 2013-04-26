using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace BookmarkManager.Model
{
    public class Tag
    {
        public Tag()
        {
            Bookmarks = new HashSet<Bookmark>();
        }

        // Primitive properties
        [Key]
        public int TagId { get; set; }
        [Required]
        public string TagName { get; set; }

        // Navigation properties
        public ICollection<Bookmark> Bookmarks { get; set; }
    }
}
