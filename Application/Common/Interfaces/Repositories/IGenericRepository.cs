using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // Basic CRUD operations
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> GetByIdAsync(object id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        
        // Advanced queries
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        
        // Modification operations (these don't save to database 
        Task<TEntity> AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        Task RemoveAsync(int id);
        Task RemoveAsync(object id);
        void RemoveRange(IEnumerable<TEntity> entities);
        
        // Include operations for navigation properties
        IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> Include(params string[] includeProperties);

        // Save changes to the database
        Task SaveChangesAsync();
    }
}