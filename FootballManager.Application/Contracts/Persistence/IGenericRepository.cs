using System.Linq.Expressions;
using FootballManager.Domain.Common;

namespace FootballManager.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : BaseEntity
{
    IQueryable<T> GetAll();
    Task<T> GetByIdAsync(int id);
    Task InsertAsync(T entity);
    Task BulkInsertAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task BulkUpdateAsync(IEnumerable<T> entities); 
    Task DeleteAsync(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> conditions);
    Task CommitAsync();
}
