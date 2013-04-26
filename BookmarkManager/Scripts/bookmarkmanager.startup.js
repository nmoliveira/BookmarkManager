///<reference path="~/Scripts/bookmarkmanager.dataservice.js"/>
///<reference path="~/Scripts/bookmarkmanager.binder.js"/>
///<reference path="~/Scripts/Libs/jquery-1.8.0.js"/>
//Contains the initial screen startup routines
var startup = function () {
    init = function () {
        getBookmarks();
        setEvents();
    },
    setEvents = function () {
        $('#btnSave').click(function () {
            var id = $('#bmId').val();
            var url = $('#bmUrl').val();
            var title = $('#bmTitle').val();
            var description = $('#bmDescription').val();
            var rating = $('#bmRating').val();
            var tagsArray = '';
            $('input[name^="btnCloseTag"]').each(function (index, element) {
                var tagToAdd = element.id;
                tagToAdd = tagToAdd.slice(11);
                if (tagsArray === '')
                    tagsArray = tagToAdd;
                else
                    tagsArray = tagsArray + ';' + tagToAdd;
            });
            dataService.saveBookmark(id, url, title, description, rating, tagsArray, getBookmarks)
            clearInputs();
        });
        $('input[name^="btnDel"]').live({
            click: function (source) {
                var bmToDeleteId = source.currentTarget.name;
                bmToDeleteId = bmToDeleteId.slice(6);
                dataService.deleteBookmark(bmToDeleteId, getBookmarks);
            }
        });
        $('input[name^="btnEdit"]').live({
            click: function (source) {
                var bmToEditId = source.currentTarget.name;
                bmToEditId = bmToEditId.slice(7);
                dataService.getBookmark(bmToEditId, loadBookmark);
            }
        });
        $('#btnSaveBookmarksLayout').click(function () {
            var position = 0;
            $('input[name^="bmTileId"]').each(function (index, element) {
                var bmToEditId = element.id;
                bmToEditId = bmToEditId.slice(8);
                dataService.saveBookmarkPosition(bmToEditId, position, getBookmarks)
                position = position + 1;
            });
        });
        $("#bookmarks").sortable();
        $("#bookmarks").disableSelection();
        $(".alert").alert();
        $('.close').live('click', function () {
            $(this).parent().hide();
        });
        $('#bmTag').live('keypress', function (event) {
            if (event.which == 13) {
                $('#bmTags').append('<div>' + event.currentTarget.value +
                                    '<input type="button" class="btn btn-link" id="btnCloseTag'
                                    + event.currentTarget.value + '" name="btnCloseTag'
                                    + event.currentTarget.value + '" value="X"/></div>');
                $('#bmTag').val('');
            }
        });
        $('input[name^="btnCloseTag"]').live('click', function () {
            $(this).parent().hide();
        });
        $('input[name^="btnDelTag"]').live({
            click: function (source) {
                var tagToDeleteId = source.currentTarget.name;
                tagToDeleteId = tagToDeleteId.slice(9);
                var params = tagToDeleteId.split('&');
                if (params.length === 2) {
                    dataService.removeTag(params[0], params[1], getBookmarks);
                }
            }
        });
    },
    clearInputs = function () {
        $('#bmId').val('');
        $('#bmUrl').val('');
        $('#bmTitle').val('');
        $('#bmDescription').val('');
        $('#bmRating').val('');
        // clear div content
        $('#bmTags').empty();
    },
    getBookmarks = function () {
        dataService.getBookmarks(loadBookmarks);
    },
    loadBookmarks = function (data) {
        bookmarksBinder.bindBookmarkList(data);
    },
    loadBookmark = function (data) {
        bookmarksBinder.bindBookmark(data);
    };
    return {
        init: init
    };
} ();