using Domain.Abstractions;
using Domain.Entities;

namespace DataAccess.Abstractions
{
    /// <summary>
    /// Inherits IRepository. Declares special methods for Users 
    /// <list type="bullet">
    ///     <item>GetByUserId(Guid userId, int year)</item>
    /// </list>
    /// </summary>
    /// <typeparam name="T">The main entity(Event) that the repository works with.</typeparam>
    public interface IEventRepository<T> : IRepository<T> where T : class, IEntity
    {
        Task<IResult<ICollection<Event>>> GetByUserId(Guid userId, int year);
    }
}
