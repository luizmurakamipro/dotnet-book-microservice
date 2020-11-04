using BooksApi.Controllers;
using BooksApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Utils
{
    public static class Connector
    {
        private static BooksController _booksController = null;

        public static BooksController getBookController()
        {
            if (_booksController == null)
            {
                var connectionString = Settings.getMongoSetting("ConnectionString");
                var databaseName = Settings.getMongoSetting("DatabaseName");
                var collectionName = Settings.getMongoSetting("BooksCollectionName");

                _booksController = new BooksController(new BookService(connectionString, databaseName, collectionName));
            }

            return _booksController;
        }
    }
}
