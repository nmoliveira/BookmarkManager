using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookmarkManager.Model;

namespace BookmarkManager.Repository
{
    public class TagRepository: RepositoryBase<BookmarkManager>, ITagRepository
    {
        public Tag getTag(int tagId)
        {
            using (var context = DataContext)
            {
                return context.Tags
                    .Where(t => t.TagId == tagId).FirstOrDefault();
            }
        }

        public Tag getTag(string tagName)
        {
            using (var context = DataContext)
            {
                return context.Tags
                    .Where(t => t.TagName == tagName).FirstOrDefault();
            }
        }

        public List<Tag> getTags()
        {
            using (var context = DataContext)
            {
                return context.Tags
                    .ToList();
            }
        }

        public bool createTag(string tagName)
        {
            Tag tag = new Tag { TagName = tagName };

            using (var context = DataContext)
            {
                context.Tags.Add(tag);
                OperationStatus status = this.Save<Tag>(tag);
                return status.Status;
            }                        
        }

        public bool deleteTag(int tagId)
        {            
            using (var context = DataContext)
            { 
                Tag tag  = context.Tags.Where(t => t.TagId == tagId).FirstOrDefault();
                if (tag != null)
                {
                    context.Tags.Remove(tag);
                    OperationStatus status = this.Save<Tag>(tag);
                    return status.Status;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool updateTag(int tagId, string tagName)
        {
            using (var context = DataContext)
            {
                Tag tag = this.getTag(tagId);
                if (tag != null)
                {
                    tag.TagName = tagName;
                    OperationStatus status = this.Update<Tag>(tag);
                    return status.Status;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
