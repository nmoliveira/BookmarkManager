using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookmarkManager.Model;
using BookmarkManager.Repository;

namespace BookmarkManager.Repository
{
    public class BookmarkRepository : RepositoryBase<BookmarkManager>, IBookmarkRepository
    {
        private TagRepository _TagRepository = new TagRepository();

        public Bookmark getBookmark(int bookmarkId)
        {
            using (var context = DataContext)
            {
                return context.Bookmarks
                    .Where(b => b.BookmarkId == bookmarkId).SingleOrDefault();
            }
        }

        public Bookmark getBookmarkWithTags(int bookmarkId)
        {
            using (var context = DataContext)
            {
                return context.Bookmarks.Include("BookmarkTags")
                    .Where(b => b.BookmarkId == bookmarkId).SingleOrDefault();
            }
        }

        public List<Bookmark> getBookmarks()
        {
            using (var context = DataContext)
            {
                return context.Bookmarks.Include("BookmarkTags")
                    .OrderBy(b => b.BookmarkPosition).ToList();            
            }
        }
        public bool Save(string id, string url, string title, string description, string rating, string[] tagArray)
        {
            if (string.IsNullOrEmpty(id))
            {
                return SaveNew(id, url, title, description, rating, tagArray);
            }
            else
            {
                return SaveOld(id, url, title, description, rating);
            }
        }

        private bool SaveOld(string id, string url, string title, string description, string rating)
        {
            Bookmark bmToEdit = getBookmark(int.Parse(id));
            bmToEdit.BookmarkUrl = url;
            bmToEdit.BookmarkTitle = title;
            bmToEdit.BookmarkDescription = description;
            bmToEdit.BookmarkRating = int.Parse(rating);
            OperationStatus status = this.Update<Bookmark>(bmToEdit);
            return status.Status;
        }

        private bool SaveNew(string id, string url, string title, string description, string rating, string[] tagArray)
        {
            Bookmark bm = new Bookmark();
            bm.BookmarkUrl = url;
            bm.BookmarkTitle = title;
            bm.BookmarkDescription = description;
            bm.BookmarkRating = rating == "" ? 0 : int.Parse(rating);
            bm.BookmarkPosition = getLastPosition() + 1;
                       
            this.DataContext.Bookmarks.Add(bm);
            OperationStatus status = this.Save<Bookmark>(bm);

            if (status.Status)
            {
                if (tagArray.Length > 0 && tagArray[0] != "")
                {
                    foreach (var tag in tagArray)
                    {

                        Tag newTag = _TagRepository.getTag(tag);

                        if (newTag == null)
                        {
                            newTag = new Tag() { TagName = tag.ToString() };
                            newTag.Bookmarks.Add(bm);
                            this.DataContext.Tags.Add(newTag);
                            this.Save<Tag>(newTag);
                        }
                        else
                        {
                            newTag.Bookmarks.Add(bm);
                            this.DataContext.Tags.Add(newTag);
                            this.Update<Tag>(newTag);

                        }
                        bm.BookmarkTags.Add(newTag);
                    }
                    status = this.Update<Bookmark>(bm);
                }
            }

            return status.Status;
        }

        public bool Delete(int bookmarkId)
        {
            Bookmark bmToDelete = this.DataContext.Bookmarks
                                      .Where(b => b.BookmarkId == bookmarkId).SingleOrDefault();
            if (bmToDelete != null)
            {
                this.DataContext.Bookmarks.Remove(bmToDelete);
                OperationStatus status = this.Save<Bookmark>(bmToDelete);
                return status.Status;
            }
            else
            {
                return false;
            }
        }
        
        public bool saveBookmarkPosition(int bookmarkId, int position)
        {
            Bookmark bmToEdit = this.DataContext.Bookmarks
                                      .Where(b => b.BookmarkId == bookmarkId).SingleOrDefault();
            if (bmToEdit != null)
            {
                bmToEdit.BookmarkPosition = position;
                OperationStatus status = this.Update<Bookmark>(bmToEdit);
                return status.Status;
            }
            else
            {
                return false;
            }
        }

        protected int getLastPosition()
        {
            Bookmark lastBookmark = this.DataContext.Bookmarks
                                                    .OrderByDescending(b => b.BookmarkPosition) 
                                                    .FirstOrDefault();

            if (lastBookmark != null)
            {
                return int.Parse(lastBookmark.BookmarkPosition.ToString());
            }
            else
            {
                return 0;
            }
        }

        public bool removeTag(string tag, int bookmarkId)
        {
            Bookmark bmToEdit = DataContext.Bookmarks.Include("BookmarkTags").Where(b => b.BookmarkId == bookmarkId).FirstOrDefault();
            
            Tag tagToRemove = DataContext.Tags.Where(t => t.TagName == tag).FirstOrDefault();

            if (bmToEdit != null && tagToRemove != null)
            {
                bmToEdit.BookmarkTags.Remove(tagToRemove);

                OperationStatus status = this.Update<Bookmark>(bmToEdit);

                return status.Status;
            }

            return false;
        }
    }
}
