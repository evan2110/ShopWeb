using System.Linq.Expressions;

namespace DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
        int pageSize = 3, int pageNumber = 1);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes);
    Task<T> GetOneAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
    Task<T> GetOneAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[]? includes);
    Task CreateAsync(T entity);
    Task RemoveAsync(T entity);
    Task UpdateAsync(T entity);
    Task SaveAsync();
}