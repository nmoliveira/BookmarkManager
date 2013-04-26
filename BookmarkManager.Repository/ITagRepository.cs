using BookmarkManager.Model;
using System.Collections.Generic;

namespace BookmarkManager.Repository
{
    public interface ITagRepository
    {
        Tag getTag(int tagId);
        Tag getTag(string tagName);
        List<Tag> getTags();
        bool createTag(string tagName);
        bool updateTag(int tagId, string tagName);
        bool deleteTag(int tagId);
    }
}
