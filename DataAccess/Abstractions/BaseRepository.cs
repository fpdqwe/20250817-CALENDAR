using DataAccess.Results;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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

        virtual public async Task<IResult<T>> Get(Guid entityId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            T? result;
            using (var context = ContextManager.CreateDatabaseContext())
            {
                result = await context.Set<T>().FindAsync(entityId);
            }
            sw.Stop();
            _logger.LogDebug($"{GetType().Name} handled get in {sw.ElapsedMilliseconds}ms.");
            return new EntityResult<T>(result, sw.ElapsedMilliseconds);
        }
        virtual public async Task<IResult<Guid>> Add(T entity)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                using (var context = ContextManager.CreateDatabaseContext())
                {
                    await context.Set<T>().AddAsync(entity);
                    await context.SaveChangesAsync();

                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfuly added entity {EntityId} in {ElapsedMs}ms",
                        GetType().Name, entity.Id, sw.ElapsedMilliseconds);
                    return new GuidResult(entity.Id, sw.ElapsedMilliseconds);
                }
            }
            catch (DbUpdateException dbEx)
            {
                sw.Stop();
                _logger.LogError(dbEx, "{TypeName} database error on add: {Message}",
                    GetType().Name, dbEx.InnerException?.Message ?? dbEx.Message);
                return new GuidResult(dbEx, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "{TypeName} Unexpected error on add}", GetType().Name);
                return new GuidResult(ex, sw.ElapsedMilliseconds);
            }
        }
        virtual public async Task<IResult<bool>> Update(T entity)
        {
            var sw = Stopwatch.StartNew();
            using (var context = ContextManager.CreateDatabaseContext())
            {
                var attachedEntity = await context.Set<T>().FindAsync(entity.Id);
                if (attachedEntity != null)
                {
                    context.Entry(attachedEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    sw.Stop();
                    _logger.LogInformation("{TypeName} failed to update database. {EntityId} does not exists. Waste: {ElapsedMs}ms.",
                        GetType().Name, entity.Id, sw.ElapsedMilliseconds);
                    return new BoolResult(false, sw.ElapsedMilliseconds);
                }
                await context.SaveChangesAsync();
            }
            sw.Stop();
            _logger.LogInformation("{TypeName} successfuly added entity {EntityId} in {ElapsedMs}ms",
                GetType().Name, entity.Id, sw.ElapsedMilliseconds);
            return new BoolResult(true, sw.ElapsedMilliseconds);
        }
        public async Task<IResult<bool>> Delete(T entity)
        {
            var sw = Stopwatch.StartNew();
            using (var context = ContextManager.CreateDatabaseContext())
            {
                try
                {
                    context.Set<T>().Remove(entity);
                    await context.SaveChangesAsync();
                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfuly deleted entity {EntityId} in {ElapsedMs}ms",
                        GetType().Name, entity.Id, sw.ElapsedMilliseconds);
                    return new BoolResult(true, sw.ElapsedMilliseconds);
                }
                catch (DbUpdateException dbEx)
                {
                    sw.Stop();
                    _logger.LogError(dbEx, "{TypeName} database error on delete: {Message}",
                        GetType().Name, dbEx.InnerException?.Message ?? dbEx.Message);
                    return new BoolResult(dbEx, sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    _logger.LogError(ex, "{TypeName} Unexpected error on add}", GetType().Name);
                    return new BoolResult(ex, sw.ElapsedMilliseconds);
                }
            }
        }
    }
}
