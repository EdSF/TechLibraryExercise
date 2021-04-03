using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Data;
using TechLibrary.Domain;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookId);
        Task<List<Book>> SearchBooks(string query);
    }

    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;

        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var queryable = _dataContext.Books.AsNoTracking().AsQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookId);
        }

        /// <summary>
        /// Allow simple LIKE pattern matching.
        ///     Not FTS...
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<Book>> SearchBooks(string query)
        {
            var searchTerms = query.Split(' ');

            var searchString = string.Join("%", query.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            var pattern = $"%{searchString}%";

            return await _dataContext.Books.Where(book => EF.Functions.Like(book.Title, pattern)
                                                          || EF.Functions.Like(book.ShortDescr, pattern)).ToListAsync();

        }
    }
}
