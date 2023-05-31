using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MovieScribe.Models;
using System.Linq.Expressions;

// The class EntityBaseRepo provides a generic implementation for the database operations 
// that can be used for any type T where T is a class that implements the IEntityBase interface
namespace MovieScribe.Data.Base
{
    public class EntityBaseRepo<T> : IEntityBaseRepo<T> where T : class, IEntityBase, new()
    {
        // Private readonly instance of the DBContext
        private readonly DBContext _context;

        // The constructor initializes the DBContext instance
        public EntityBaseRepo(DBContext context)
        {
            _context = context;
        }

        // AddAsync adds a new entity of type T to the database asynchronously
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // DeleteAsync removes an entity of type T from the database asynchronously
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(n => n.ID == id);
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        // GetAllAsync retrieves all entities of type T from the database asynchronously
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _context.Set<T>().ToListAsync();
            return result;
        }

        // This GetAllAsync version accepts an array of expressions to include related entities in the result
        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            // Aggregate function is used to Include all the related entities defined in includeProperties
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.ToListAsync();
        }

        // GetByIdAsync retrieves an entity of type T with a specific ID from the database asynchronously
        public async Task<T> GetByIdAsync(int id)
        {
            var results = await _context.Set<T>().FirstOrDefaultAsync(n => n.ID == id);
            return results;
        }

        // UpdateAsync updates an existing entity of type T in the database asynchronously
        public async Task UpdateAsync(int id, T entity)
        {
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Search function provides the feature to search the entities by their names.
        // The type of the entity is checked using typeof and appropriate query is executed.
        // This function highlights the power of generics where a single function can handle multiple types.
        public async Task<IEnumerable<T>> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new List<T>();
            }
            query = query.ToLower();

            if (typeof(T) == typeof(ActorModel))
            {
                var actors = _context.Set<ActorModel>().AsQueryable();
                actors = actors.Where(a => a.Name.ToLower().Contains(query));
                return (IEnumerable<T>)(object)await actors.ToListAsync();
            }
            else if (typeof(T) == typeof(DistributorModel))
            {
                var distributors = _context.Set<DistributorModel>().AsQueryable();
                distributors = distributors.Where(d => d.Name.ToLower().Contains(query));
                return (IEnumerable<T>)(object)await distributors.ToListAsync();
            }
            else if (typeof(T) == typeof(ProducerModel))
            {
                var producers = _context.Set<ProducerModel>().AsQueryable();
                producers = producers.Where(p => p.Name.ToLower().Contains(query));
                return (IEnumerable<T>)(object)await producers.ToListAsync();
            }
            else if (typeof(T) == typeof(StudioModel))
            {
                var studios = _context.Set<StudioModel>().AsQueryable();
                studios = studios.Where(s => s.Name.ToLower().Contains(query));
                return (IEnumerable<T>)(object)await studios.ToListAsync();
            }
            else if (typeof(T) == typeof(WriterModel))
            {
                var writers = _context.Set<WriterModel>().AsQueryable();
                writers = writers.Where(w => w.Name.ToLower().Contains(query));
                return (IEnumerable<T>)(object)await writers.ToListAsync();
            }

            return new List<T>();
        }
    }
}
