using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Domain.Common;

namespace UserService.Infrastructure.Persistence;
// Base repository implementation for basic CRUD operations. Anything else should be implemented in corresponding repository.
internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected ApplicationDbContext DbContext;
    protected readonly DbSet<TEntity> Entities;

    public Repository(ApplicationDbContext context)
    {
        DbContext = context;
        Entities = DbContext.Set<TEntity>();

    }

    public async Task<TEntity?> GetByIdAsync(object id)
    {
        return await Entities.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
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

    public async Task CommitChanges()
    {
        await DbContext.SaveChangesAsync();
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}