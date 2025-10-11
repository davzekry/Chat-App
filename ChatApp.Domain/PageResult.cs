namespace ChatApp.Domain
{
    public class PageResult<T>
    {
        public IQueryable<T>? Items { get; set; }
        public IEnumerable<T>? Data { get; set; }
        public int TotalCount { get; set; }    // Total number of records
        public int PageNumber { get; set; }    // Current page number
        public int PageSize { get; set; }      // Number of items per page
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);    // Total number of pages
        public int Count => Data?.Count() ?? 0;  // Number of items in the current page
        public bool HasNextPage => PageNumber < TotalPages;  // True if next page exists
        public bool HasPreviousPage => PageNumber > 1;

    }
}
