using Domain.Abstractions;

namespace DataAccess.Abstractions
{
    /// <summary>
    /// Inherits IRepository. Declares special methods for Users 
    /// <list type="bullet">
    ///     <item>GetByLogin(string login)</item>
    ///     <item>GetPasswordByLogin(string login)</item>
    /// </list>
    /// </summary>
    /// <typeparam name="T">The main entity(User) that the repository works with.</typeparam>
    public interface IUserRepository<T> : IRepository<T> where T : class, IEntity
    {
        public Task<IResult<T>> GetByLogin(string login);
        public Task<IResult<string>> GetPasswordByLogin(string login);
    }
}
