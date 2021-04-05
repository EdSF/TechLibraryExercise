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
        Task<PageList<Book>> GetBooksAsync(int page, int itemsPerPage);
        Task<Book> GetBookByIdAsync(int bookId);
        Task<PageList<Book>> SearchBooks(string query, int page, int itemsPerPage);
        Task UpdateBook(Book book);
    }

    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;

        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //public async Task<List<Book>> GetBooksAsync()
        //{
        //    var queryable = _dataContext.Books.AsNoTracking().AsQueryable();

        //    return await queryable.ToListAsync();
        //}

        public async Task<PageList<Book>> GetBooksAsync(int page, int itemsPerPage)
        {
            return await PageList<Book>.CreateAsync(_dataContext.Books.AsNoTracking(), page, itemsPerPage);
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
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public async Task<PageList<Book>> SearchBooks(string query, int page, int itemsPerPage)
        {
            var searchTerms = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var searchString = string.Join("%", searchTerms);

            var pattern = $"%{searchString}%";

            return await PageList<Book>.CreateAsync(_dataContext.Books.Where(book =>
                EF.Functions.Like(book.Title, pattern)
                || EF.Functions.Like(book.ShortDescr, pattern)), page, itemsPerPage);

        }

        public async Task UpdateBook(Book book)
        {

            var bookToUpdate = await _dataContext.Books.FirstOrDefaultAsync(b => b.BookId == book.BookId);
            if (bookToUpdate == null)
                throw new DbUpdateException("Book to update not found");

            bookToUpdate.ShortDescr = book.ShortDescr;
            bookToUpdate.ISBN = book.ISBN;
            bookToUpdate.LongDescr = book.LongDescr;
            bookToUpdate.PublishedDate = book.PublishedDate;
            bookToUpdate.ThumbnailUrl = book.ThumbnailUrl;
            bookToUpdate.Title = book.Title;
            await _dataContext.SaveChangesAsync();


        }
    }
}
