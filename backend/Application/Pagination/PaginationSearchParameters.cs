using Domain.Entities;
using NetTopologySuite.Utilities;

namespace Application.Pagination
{
    public class PaginationSearchParameters
    {
        public string? SearchQuery { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 8;

        public string GetCacheKey()
        {
            var parts = new List<string>
            {
                $"searchQuery={SearchQuery}",
                $"page={Page}",
                $"pageSize={PageSize}",
            };

            return string.Join(";", parts);
        }
    }
}
