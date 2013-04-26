using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using BookmarkManager.Model;

namespace BookmarkManager.Repository
{
    public class BookmarkManager: DbContext, IDisposedTracker
    {
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public bool IsDisposed { get; set; }

        // can be used to do changes to our model 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Examples

            // this is only needed when entity name doesn't match with table name
            modelBuilder.Entity<Bookmark>().ToTable("Bookmarks");
            modelBuilder.Entity<Tag>().ToTable("Tags");          

            base.OnModelCreating(modelBuilder);
        }

        // Delete all bookmarks
        public int DeleteBookmarks()
        {
            // Call store procedure
            return base.Database.ExecuteSqlCommand("DeleteBookmarks");
        }

        // Delete Tags
        public int DeleteTags()
        {
            // Call store procedure
            return base.Database.ExecuteSqlCommand("DeleteTags");
        }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
