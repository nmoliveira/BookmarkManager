using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookmarkManager.Repository;
using Microsoft.Practices.Unity;

namespace BookmarkManager.Controllers
{
    public class DataServiceController : Controller
    {
        IBookmarkRepository _BookmarkRepository;
        ITagRepository _TagRepository;

        public DataServiceController(IBookmarkRepository bmRepo, ITagRepository tgRepo)
        {
            _BookmarkRepository = bmRepo;
            _TagRepository = tgRepo;
        }

        public ActionResult GetBookmark(int bookmarkId)
        {
            var bm = _BookmarkRepository.getBookmark(bookmarkId);
            return Json(bm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBookmarks()
        {
            var bms = _BookmarkRepository.getBookmarks();

            var list = bms
                    .Select(b => new
                    {
                        BookmarkId = b.BookmarkId,
                        BookmarkUrl = b.BookmarkUrl,
                        BookmarkTitle = b.BookmarkTitle,
                        BookmarkDescription = b.BookmarkDescription,
                        BookmarkRating = b.BookmarkRating,
                        BookmarkPosition = b.BookmarkPosition,
                        BookmarkTags = b.BookmarkTags.Select(t => t.TagName).ToList()
                    })
                    .OrderBy(b => b.BookmarkPosition).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTag(int tagId)
        {
            var tg = _TagRepository.getTag(tagId);
            return Json(tg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTags()
        {
            var tgs = _TagRepository.getTags();
            return Json(tgs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveBookmark(string id, string url, string title, string description, string rating, string tagArray)
        {
            string[] tags = tagArray.Split(';');
            var bm = _BookmarkRepository.Save(id, url, title, description, rating, tags);
            return Json(bm, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteBookmark(int bookmarkId)
        {
            var bm = _BookmarkRepository.Delete(bookmarkId);
            return Json(bm, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditBookmark(int bookmarkId)
        {
            var bm = _BookmarkRepository.getBookmark(bookmarkId);
            return Json(bm, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveBookmarkPosition(int bookmarkId, int position)
        {
            var bm = _BookmarkRepository.saveBookmarkPosition(bookmarkId, position);
            return Json(bm, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveTag(string tag, int bookmarkId)
        {
            var bm = _BookmarkRepository.removeTag(tag, bookmarkId);
            return Json(bm, JsonRequestBehavior.AllowGet);
        }
    }
}
