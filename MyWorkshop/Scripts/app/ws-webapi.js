(function () {

    var ApiUri = '/api/books';

    $(document).ready(function () {

        showAllBooks();

        $('#wa-show-all').on('click', function (event) {
            event.preventDefault();
            showAllBooks();
        });

        $('#wa-find').on('click', function (event) {
            event.preventDefault();
            findBook();
        });

        $('#wa-show-add').on('click', function () {
            event.preventDefault();

            $('#wa-title').val('');
            $('#wa-author').val('');
            $('#wa-year').val('');
            $('#wa-cover').val('');

            $('#wa-block-edit').css('display', 'none');
            $('#wa-block-add').css('display', 'block');
        });

        $('#wa-add').on('click', function (event) {
            event.preventDefault();
            addBook();
        });

        $('#wa-edit-selected').on('click', function (event) {
            event.preventDefault();
            $('#wa-block-add').css('display', 'none');
            preEditBook();
        });

        $('#wa-edit').on('click', function (event) {
            event.preventDefault();
            editBook();
        });

        $('#wa-del-selected').on('click', function (event) {
            event.preventDefault();
            deleteBook();
        });
    });


    // helper func for calling jquery ajax method
    function ajaxHelper(uri, method, data) {
        $('#wa-display-error').toggleClass('alert', false).html('');
        return $.getJSON({
            type: method,
            url: uri,
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $('#wa-display-error').toggleClass('alert', true).html('<strong>Error:</strong> ' + errorThrown);
        });
    }

    // fills new row with data and append it to table
    function appendRow(item) {
        var row = $("<tr/>");
        row.append($("<th/>").html(item.id));
        row.append($("<td/>").html(item.title));
        row.append($("<td/>").html(item.author));
        row.append($("<td/>").html(item.year));
        row.append($("<td/>").html(item.cover));
        body += row.prop('outerHTML');
    }

    // hides all additional blocks and errors
    function basicView() {
        $('#wa-block-add').css('display', 'none');
        $('#wa-block-edit').css('display', 'none');
        $('#wa-display-error').toggleClass('alert', false).html('');
    }

    // add bgcolor and class 'selected' for chosen row in table
    function clickableRow() {
        $('table#wa-table.table tbody tr').click(function () {
            $(this).addClass('selected').siblings().removeClass('selected');
        });
    }

    var body = '';

    // show all books
    function showAllBooks() {
        ajaxHelper(ApiUri, 'GET').done(function (data) {
            body = '';
            $.each(data, function (key, item) {
                appendRow(item);
            });
            $('#wa-table tbody').html(body);
            clickableRow();
            basicView();
        });
    }

    // find book by id
    function findBook() {
        var id = $('#wa-searching-id').val();
        ajaxHelper(ApiUri + "/" + id, 'GET').done(function (item) {
            body = '';
            appendRow(item);
            $('#wa-table tbody').html(body);
            clickableRow();
            basicView();
        });
    }

    // add new book
    function addBook() {
        var newbook = {
            Title: $('#wa-title').val(),
            Author: $('#wa-author').val(),
            Year: $('#wa-year').val(),
            Cover: $('#wa-cover').val()
        };

        ajaxHelper(ApiUri, 'POST', newbook).done(function (data) {
            showAllBooks();
        });
    }

    // prepare fields for editing
    function preEditBook() {
        var id = $('table#wa-table.table tbody tr.selected th:first').html();
        if (id === undefined) {
            alert("You have to select row first");
            return;
        }

        ajaxHelper(ApiUri + '/' + id, 'GET').done(function (data) {
            $('#wa-edit-id').val(data.id);
            $('#wa-edit-title').val(data.title);
            $('#wa-edit-author').val(data.author);
            $('#wa-edit-year').val(data.year);
            $('#wa-edit-cover').val(data.cover);
            $('#wa-block-edit').css('display', 'block');
        });
    }

    // edit selected row
    function editBook() {
        var editbook = {
            Id: $('#wa-edit-id').val(),
            Title: $('#wa-edit-title').val(),
            Author: $('#wa-edit-author').val(),
            Year: $('#wa-edit-year').val(),
            Cover: $('#wa-edit-cover').val()
        };

        ajaxHelper(ApiUri + '/' + $("#wa-edit-id").val(), 'PUT', editbook).done(function (data) {
            showAllBooks(); // add one row <?> refresh full table
            basicView();
        });
    }

    // remove selected row
    function deleteBook() {
        var id = $('table#wa-table.table tbody tr.selected th:first').html();
        if (id === undefined) {
            alert("You have to select row first");
            return;
        }
        ajaxHelper(ApiUri + '/' + id, 'DELETE').done(function (data) {
            $('table#wa-table.table tbody tr.selected').fadeOut(500, function () {
                $(this).remove();
            });
            basicView();
        });
    }
})();

