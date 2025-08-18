using Domain.Abstractions;

namespace DataAccess.Abstractions
{
    public interface IRepository<T> where T : class, IEntity
    {
        IContextManager ContextManager { get; }
        Task<T?> Get(Guid entityId);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
