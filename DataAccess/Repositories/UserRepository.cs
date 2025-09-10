using DataAccess.Abstractions;
using DataAccess.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly IContextManager _contextManager;
        private readonly ILogger _logger;
        public UserRepository(IContextManager contextManager, ILogger<UserRepository> logger)
        {
            _contextManager = contextManager;
            _logger = logger;
            _logger.LogDebug($"New instance of {GetType().Name} was initialized");
        }

        public IContextManager ContextManager => _contextManager;

        public async Task<IResult<Guid>> Add(User entity)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = _contextManager.CreateDatabaseContext())
                {
                    await context.Users.AddAsync(entity);
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
        public async Task<IResult<bool>> Delete(User entity)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = _contextManager.CreateDatabaseContext())
                {
                    context.Users.Remove(entity);
                    await context.SaveChangesAsync();
                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfuly deleted entity {EntityId} in {ElapsedMs}ms",
                        GetType().Name, entity.Id, sw.ElapsedMilliseconds);
                    return new BoolResult(true, sw.ElapsedMilliseconds);
                }
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
                _logger.LogError(ex, "{TypeName} Unexpected error on delete}", GetType().Name);
                return new BoolResult(ex, sw.ElapsedMilliseconds);
            }
        }
        public async Task<IResult<User>> Get(Guid entityId)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = _contextManager.CreateDatabaseContext())
                {
                    var result = await context.Users.FirstOrDefaultAsync(x => x.Id == entityId);

                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfuly got entity {EntityId} in {ElapsedMs}ms",
                        GetType().Name, entityId, sw.ElapsedMilliseconds);
                    return new EntityResult<User>(result, sw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "{TypeName} Unexpected error on get}", GetType().Name);
                return new EntityResult<User>(ex, sw.ElapsedMilliseconds);
            }
        }

        public async Task<IResult<User>> GetByLogin(string login)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = _contextManager.CreateDatabaseContext())
                {
                    var result = await context.Users.FirstOrDefaultAsync(x => x.Login == login);

                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfuly got entity {EntityId} in {ElapsedMs}ms",
                        GetType().Name, login, sw.ElapsedMilliseconds);
                    return new EntityResult<User>(result, sw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "{TypeName} Unexpected error on get}", GetType().Name);
                return new EntityResult<User>(ex, sw.ElapsedMilliseconds);
            }
        }

        public async Task<IResult<string>> GetPasswordByLogin(string login)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = _contextManager.CreateDatabaseContext())
                {
                    var result = await context.Users.Where(x => x.Login == login)
                        .Select(x => x.Password).FirstOrDefaultAsync();
                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfuly got password" +
                        " entity {EntityId} in {ElapsedMs}ms",
                        GetType().Name, login, sw.ElapsedMilliseconds);
                    return new EntityResult<string>(result, sw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "{TypeName} Unexpected error on GetPassword}", GetType().Name);
                return new EntityResult<string>(ex, sw.ElapsedMilliseconds);
            }
        }

        public async Task<IResult<bool>> Update(User entity)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var context = _contextManager.CreateDatabaseContext())
                {
                    context.Users.Update(entity);
                    await context.SaveChangesAsync();
                    sw.Stop();
                    _logger.LogInformation("{TypeName} successfuly updated entity {EntityId} in {ElapsedMs}ms",
                        GetType().Name, entity.Id, sw.ElapsedMilliseconds);
                    return new BoolResult(true, sw.ElapsedMilliseconds);
                }
            }
            catch (DbUpdateException dbEx)
            {
                sw.Stop();
                _logger.LogError(dbEx, "{TypeName} database error on update: {Message}",
                    GetType().Name, dbEx.InnerException?.Message ?? dbEx.Message);
                return new BoolResult(dbEx, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "{TypeName} Unexpected error on update}", GetType().Name);
                return new BoolResult(ex, sw.ElapsedMilliseconds);
            }
        }
    }
}
