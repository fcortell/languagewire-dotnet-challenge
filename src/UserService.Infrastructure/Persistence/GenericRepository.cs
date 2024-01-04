using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Domain.Common;

namespace UserService.Infrastructure.Persistence;
// Base repository implementation for basic CRUD operations. Anything else should be implemented in corresponding repository.
public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected ApplicationDbContext DbContext;
    protected readonly DbSet<TEntity> Entities;

    public GenericRepository(ApplicationDbContext context)
    {
        DbContext = context;
        Entities = DbContext.Set<TEntity>();

    }

    public async Task<TEntity?> GetByIdAsync(long id)
    {
        return await Entities.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<TEntity?>> GetAllAsync()
    {
        return await Entities.AsQueryable().ToListAsync();
    }

    public void Insert(TEntity entity)
    {
        Entities.Add(entity);
    }
    public void Update(TEntity entity)
    {
        Entities.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        Entities.Remove(entity);
    }

    public async Task CommitChangesAsync(CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity?>> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return await Entities.Where(predicate).ToListAsync();
    }

}