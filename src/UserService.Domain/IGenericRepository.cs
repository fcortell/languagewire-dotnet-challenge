using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity?> GetByIdAsync(long id);
        Task<IEnumerable<TEntity?>> GetAllAsync();
        Task<IEnumerable<TEntity?>> Find(Expression<Func<TEntity, bool>> predicate);
        Task CommitChangesAsync(CancellationToken cancellationToken);

    }
}
