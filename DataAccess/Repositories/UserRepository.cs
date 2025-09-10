using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository<User>
    {
        public UserRepository(IContextManager contextManager, ILogger<UserRepository> logger)
            : base(contextManager, logger) { }
        public async Task<IResult<Guid>> Add(User entity)
        {
            return await ExecuteOperation(async context =>
            {
                await context.Users.AddAsync(entity);
                await context.SaveChangesAsync();
                return entity.Id;
            }, nameof(Add));
        }
        public async Task<IResult<bool>> Delete(User entity)
        {
            return await ExecuteOperation(async context =>
            {
                context.Users.Attach(entity);
                context.Users.Remove(entity);
                try
                {
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }
            }, nameof(Delete));
        }
        public async Task<IResult<User>> Get(Guid entityId)
        {
            return await ExecuteOperation(async context =>
            {
                var result = await context.Users.FindAsync(entityId);
                if (result == null) return (null, ValidationResult.Invalid($"User id: {entityId} does not exists"));
                else return (result, ValidationResult.Valid());
            }, nameof(Get));
        }
        public async Task<IResult<User>> GetByLogin(string login)
        {
            return await ExecuteOperation(async context =>
            {
                var result = await context.Users.FirstOrDefaultAsync(x => x.Login == login);
                if (result == null) return (null, ValidationResult.Invalid($"User login: {login} does not exists"));
                else return (result, ValidationResult.Valid());
            }, nameof(GetByLogin));
        }
        public async Task<IResult<string>> GetPasswordByLogin(string login)
        {
            return await ExecuteOperation(async context =>
            {
                var result = await context.Users.Where(x => x.Login == login)
                    .Select(x => x.Password).FirstOrDefaultAsync();
                if (result == null) return (null, ValidationResult.Invalid($"User login: {login} does not exists"));
                else return (result, ValidationResult.Valid());
            }, nameof(GetPasswordByLogin));
        }
        public async Task<IResult<bool>> Update(User entity)
        {
            return await ExecuteOperation(async context =>
            {
                var existingUser = await context.Users
                    .FirstOrDefaultAsync(u => u.Id == entity.Id);

                if (existingUser == null)
                    return (false, ValidationResult.Invalid("User does not exists"));
                if (!string.IsNullOrEmpty(entity.Login) && entity.Login != existingUser.Login)
                    return (false, ValidationResult.Invalid("Login cannot be changed"));
                if (entity.DateRegistered != default && entity.DateRegistered != existingUser.DateRegistered)
                    return (false, ValidationResult.Invalid("Registration date cannot be changed"));

                if (!string.IsNullOrEmpty(entity.Name)) existingUser.Name = entity.Name;
                if (!string.IsNullOrEmpty(entity.Lastname)) existingUser.Lastname = entity.Lastname;
                if (!string.IsNullOrEmpty(entity.Password)) existingUser.Password = entity.Password;
                if (entity.Zodiac != default) existingUser.Zodiac = entity.Zodiac;
                if (entity.DateOfBirth != default) existingUser.DateOfBirth = entity.DateOfBirth;

                await context.SaveChangesAsync();
                return (true, ValidationResult.Valid());
            }, nameof(Update));
        }
    }
}
