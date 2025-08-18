using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly IContextManager _contextManager;
        private readonly ILogger _logger;
        public UserRepository(IContextManager contextManager, ILogger logger)
        {
            _contextManager = contextManager;
            _logger = logger;
        }

        public IContextManager ContextManager => _contextManager;

        public async Task<bool> Add(User entity)
        {
            try
            {
                using (var context = _contextManager.GenerateDatabaseContext())
                {
                    await context.Users.AddAsync(entity);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(User entity)
        {
            try
            {
                using (var context = _contextManager.GenerateDatabaseContext())
                {
                    context.Users.Remove(entity);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<User?> Get(Guid entityId)
        {
            try
            {
                using (var context = _contextManager.GenerateDatabaseContext())
                {
                    return await context.Users.FirstOrDefaultAsync(x => x.Id == entityId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<User?> GetByLogin(string login)
        {
            try
            {
                using (var context = _contextManager.GenerateDatabaseContext())
                {
                    return await context.Users.FirstOrDefaultAsync(x => x.Login == login);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<string?> GetPasswordByLogin(string login)
        {
            try
            {
                using (var context = _contextManager.GenerateDatabaseContext())
                {
                    return await context.Users.Where(x => x.Login == login)
                        .Select(x => x.Password).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<bool> Update(User entity)
        {
            try
            {
                using (var context = _contextManager.GenerateDatabaseContext())
                {
                    context.Users.Update(entity);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
