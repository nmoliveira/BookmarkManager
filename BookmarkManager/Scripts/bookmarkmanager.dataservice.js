//Encapsulates data calls to server (AJAX calls)
var dataService = new function () {
    // service base points to our DataService Controller
    var serviceBase = '/DataService/',

        getBookmarks = function (callback) {
            $.getJSON(serviceBase + 'GetBookmarks', function (data) {
                callback(data);
            });
        },
        saveBookmark = function (pid, purl, ptitle, pdescription, prating, ptagArray, callback) {
            $.getJSON(serviceBase + 'SaveBookmark',
                { id: pid, url: purl, title: ptitle, description: pdescription, rating: prating, tagArray: ptagArray },
                function (data) {
                    if (data) {
                        showSuccess();
                        callback();
                    }
                    else {
                        showError();
                    }
                });
        },
        deleteBookmark = function (bookmarkId, callback) {
            $.getJSON(serviceBase + 'DeleteBookmark', { bookmarkId: bookmarkId }, function (data) {
                if (data) {
                    showSuccess();
                    callback(data);
                }
                else {
                    showError();
                }
            });
        },
        getBookmark = function (bookmarkId, callback) {
            $.getJSON(serviceBase + 'GetBookmark', { bookmarkId: bookmarkId }, function (data) {
                if (data) {
                    callback(data);
                }
            });
        },
        saveBookmarkPosition = function saveBookmarkPosition(bookmarkId, position, callback) {
            $.getJSON(serviceBase + 'SaveBookmarkPosition', { bookmarkId: bookmarkId, position: position }, function (data) {
                if (data) {
                    showSuccess();
                    callback(data);
                }
                else {
                    showError();
                }
            });
        },
        showSuccess = function () {
            $(".alert-error").hide();
            $(".alert-success").show();
        },
        showError = function () {
            $(".alert-success").hide();
            $(".alert-error").show();
        },
        getTags = function (callback) {
            $.getJSON(serviceBase + 'GetTags', function (data) {
                callback(data);
            });
        },
        removeTag = function (tag, bookmarkId, callback) {
            $.getJSON(serviceBase + 'RemoveTag', {tag: tag, bookmarkId: bookmarkId}, function (data) {
                callback(data);
            });
        }
    return {
        getBookmarks: getBookmarks,
        saveBookmark: saveBookmark,
        deleteBookmark: deleteBookmark,
        getBookmark: getBookmark,
        saveBookmarkPosition: saveBookmarkPosition,
        getTags: getTags,
        removeTag: removeTag
    };

} ();