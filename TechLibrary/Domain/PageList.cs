using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TechLibrary.Domain
{
    public class PageList<T>
    {
        public int Page { get; }
        public int TotalPages { get; }

        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

        public List<T> Items { get; } = new List<T>();

        public PageList(List<T> items, int count, int page, int itemsPerPage)
        {
            Page = page;
            TotalPages = (int)Math.Ceiling(count / (double)itemsPerPage);
            Items.AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync(IQueryable<T> data, int page, int itemsPerPage)
        {
            var count = await data.CountAsync();
            var items = await data.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
            return new PageList<T>(items, count, page, itemsPerPage);
        }
    }
}
