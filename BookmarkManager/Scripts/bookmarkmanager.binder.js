//Handles loading jQuery templates dynamically from server
//and rendering them based upon data
var bookmarksBinder = function () {
    var templateBase = '/Templates/',

    bindBookmarkList = function (data) {
        $.get(templateBase + 'BookmarkTiles.html', function (template) {
            // get div to append content and clear its content
            var divToAppend = $('#bookmarks').empty();
            // append template to div
            divToAppend.append(template);
            // render template
            $('#BookmarkList').tmpl(data).appendTo(divToAppend);
        });
    },
    bindBookmark = function (data) {
        $('#bmId').val(data.BookmarkId);
        $('#bmUrl').val(data.BookmarkUrl);
        $('#bmTitle').val(data.BookmarkTitle);
        $('#bmDescription').val(data.BookmarkDescription);
        $('#bmRating').val(data.BookmarkRating);
    };
    return {
        bindBookmarkList: bindBookmarkList,
        bindBookmark: bindBookmark
    };
} ();