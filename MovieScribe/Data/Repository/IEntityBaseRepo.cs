using MovieScribe.Models;
using System.Linq.Expressions;

namespace MovieScribe.Data.Base
{
    public interface IEntityBaseRepo<T> where T : class, IEntityBase, new()
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);

        Task<T> GetByIdAsync(int id);

        Task AddAsync(T entity);

        Task UpdateAsync(int id, T entity);

        Task DeleteAsync(int id);

        Task<IEnumerable<T>> Search(string query);
    }
}
