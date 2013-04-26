using BookmarkManager.Model;
using System.Collections.Generic;

namespace BookmarkManager.Repository
{   
    public interface IBookmarkRepository
    {
        Bookmark getBookmark(int bookmarkId);
        List<Bookmark> getBookmarks();
        bool Save(string id, string url, string title, string description, string rating, string[] tagArray);
        bool Delete(int bookmarkId);
        bool saveBookmarkPosition(int bookmarkId, int position);
        bool removeTag(string tag, int bookmarkId);
    }
}
