using FootballManager.Application.Utilities;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Application.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<(List<T>, int)> Page<T>(this IQueryable<T> query, Pagination pagination, CancellationToken cancellationToken = default)
        {
            var count = await query.CountAsync(cancellationToken);

            var (from, to) = GetFromAndToParams(pagination);

            var items = await query.Skip(from).Take(to).ToListAsync(cancellationToken);

            return (items, count);
        }

        public static (int From, int To) GetFromAndToParams(Pagination pagination)
        {
            return ((pagination.PageNumber - 1) * pagination.PageSize, pagination.PageSize);
        }
    }
}
