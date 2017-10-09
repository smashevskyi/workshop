(function () {
    var BookViewModel = function () {
        var self = this;
        var booksUri = '/api/books/';

        self.id = ko.observable("");
        self.title = ko.observable("");
        self.author = ko.observable("");
        self.year = ko.observable("");
        self.cover = ko.observable("");

        var book = {
            Id: self.id,
            Title: self.title,
            Author: self.author,
            Year: self.year,
            Cover: self.cover
        };

        self.book = ko.observable();
        self.books = ko.observableArray();
        self.error = ko.observable();
        
        function koAjaxHelper(uri, method, data) {
            self.error(''); // Clear error message
            return $.ajax({
                type: method,
                url: uri,
                dataType: 'json',
                contentType: 'application/json',
                data: data ? ko.toJSON(data) : null
            }).fail(function (jqXHR, textStatus, errorThrown) {
                self.error(errorThrown);
            });
        }

        self.getBooks = function () {
            koAjaxHelper(booksUri, 'GET').done(function (data) {
                self.books(data);
            });
        }

        self.addBook = function () {
            var newBook = book;
            delete newBook.Id;
            //var createBook = ko.toJSON(newBook);
            koAjaxHelper(booksUri, 'POST', newBook).done(function (data) {
                self.books.push(data);
                self.title("");
                self.author("");
                self.year("");
                self.cover("");
            });
        }

        // Edit book
        self.edit = function (Book) {
            self.book(Book);
        }

        self.editBook = function () {
            var updBook = self.book();
            koAjaxHelper(booksUri + '/' + updBook.id, 'PUT', updBook).done(function (data) {
                self.getBooks();
                self.book(null);
            });
        }

        // delete book
        self.delBook = function (Book) {
            var id = Book.id;
            koAjaxHelper(booksUri + '/' + id, 'DELETE').done(function (data) {
                self.books.remove(Book);
            });
        }

        // Reset book
        self.reset = function () {
            self.title("");
            self.author("");
            self.year("");
            self.cover("");
        }

        // Cancel book
        self.cancel = function () {
            self.book(null);
        };

        // fetch initial data
        koAjaxHelper(booksUri, 'GET').done(function (data) {
            self.books(data);
        });
    };

    ko.applyBindings(new BookViewModel());
})();
