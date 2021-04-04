using System.Collections.Generic;

namespace TechLibrary.Contracts.Responses
{
    public class BookResponse
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublishedDate { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Descr { get; set; }
    }

    public class PagedBookResponse
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems  { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public List<BookResponse> Books { get; set; }
    }
}
