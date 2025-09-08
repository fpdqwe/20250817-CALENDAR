using Domain.Abstractions;
using Microsoft.Extensions.Logging;

namespace DataAccess.Abstractions
{
    /// <summary>
    /// Base repository for fast implementation of CRUD operations.
    /// Requires clarification for complex entities 
    /// (in such cases, it is better to inherit from IRepository directly)
    /// </summary>
    /// <typeparam name="T">The main entity that the repository works with.</typeparam>
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        public IContextManager ContextManager { get; private set; }
        protected ILogger _logger;
        public BaseRepository(IContextManager contextManager, ILogger logger)
        {
            ContextManager = contextManager;
            _logger = logger;
        }

        virtual public async Task<T?> Get(Guid entityId)
        {
            using (var context = ContextManager.CreateDatabaseContext())
            {
                return await context.Set<T>().FindAsync(entityId);
            }
        }
        virtual public async Task<bool> Add(T entity)
        {
            using (var context = ContextManager.CreateDatabaseContext())
            {
                var iDbEntity = entity as IEntity;
                if (iDbEntity == null) throw new ArgumentException("Entity should be IDbEntity type", "entity");

                await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
            }
            return true;
        }
        virtual public async Task<bool> Update(T entity)
        {
            using (var context = ContextManager.CreateDatabaseContext())
            {
                var iDbEntity = entity as IEntity;
                if (iDbEntity == null) throw new ArgumentException("Entity should be IDbEntity type", "entity");

                var attachedEntity = await context.Set<T>().FindAsync(iDbEntity.Id);
                if (attachedEntity != null)
                {
                    context.Entry(attachedEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    _logger.LogCritical("attachedEntity was null during entity Update");
                    return false;
                }
                await context.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> Delete(T entity)
        {
            using (var context = ContextManager.CreateDatabaseContext())
            {
                try
                {
                    var iDbEntity = entity as IEntity;
                    if (iDbEntity == null) throw new ArgumentException("Entity should be IDbEntity type", "entity");

                    context.Set<T>().Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception) { return false; }
            }
        }
    }
}
