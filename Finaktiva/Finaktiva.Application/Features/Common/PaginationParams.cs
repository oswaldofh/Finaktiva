namespace Finaktiva.Aplication.Features.Common
{
    public class PaginationParams
    {
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public string? Search { get; set; }

        public PaginationParams(string? sort, int? pageIndex, int? pageSize, string? search)
        {
            Sort = sort;
            PageIndex = pageIndex is null ? 1 : pageIndex.Value;
            PageSize = pageSize is null ? 10 : pageSize.Value;
            Search = search;
        }
        public PaginationParams()
        {

        }
    }
}
