using System.Linq.Expressions;

namespace UserService.Domain
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task CommitChangesAsync(CancellationToken cancellationToken);

        void Delete(TEntity entity);

        Task<IEnumerable<TEntity?>> Find(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity?>> GetAllAsync();

        Task<TEntity?> GetByIdAsync(long id);

        void Insert(TEntity entity);

        void Update(TEntity entity);
    }
}