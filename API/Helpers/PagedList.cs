using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items, int count, int PageNumber, int pageSize)
        {
            CurrentPage = PageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public int TotalCount { get; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,
            int PageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((PageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, PageNumber, pageSize);
        }
    }
}