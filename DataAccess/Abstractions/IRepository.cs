using Domain.Abstractions;

namespace DataAccess.Abstractions
{
    /// <summary>
    /// The basic interface of the repository.
    /// All repositories must inherit from this interface.
    /// Implements CRUD operations, as well as a СontextManager.
    /// </summary>
    /// <typeparam name="T">The main entity that the repository works with.</typeparam>
    public interface IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// An auxiliary class for fast creating DbContext instances.
        /// It can be implemented as a singleton.
        /// </summary>
        IContextManager ContextManager { get; }
        /// <summary>
        /// Gets an entity by its primary key.
        /// </summary>
        /// <param name="entityId">Requested entity id.</param>
        /// <returns>Repo entity.</returns>
        Task<T?> Get(Guid entityId);
        /// <summary>
        /// Adds entity.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        /// <returns>Success status. Should be false in error cases.</returns>
        Task<bool> Add(T entity);
        /// <summary>
        /// Updates existing entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>Success status. Should be false in error cases.</returns>
        Task<bool> Update(T entity);
        /// <summary>
        /// Deletes existing entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <returns>Success status. Should be false in error cases.</returns>
        Task<bool> Delete(T entity);
    }
}
