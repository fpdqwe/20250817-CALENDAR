using Domain.Abstractions;

namespace DataAccess.Abstractions
{
    public interface IUserRepository<T> : IRepository<T> where T : class, IEntity
    {
        public Task<T?> GetByLogin(string login);
        public Task<string?> GetPasswordByLogin(string login);
    }
}
