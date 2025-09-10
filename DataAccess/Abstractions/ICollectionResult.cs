using Domain.Abstractions;

namespace DataAccess.Abstractions
{
    public interface ICollectionResult<T> : IResult<T> where T : class, IEntity
    {
        public ICollection<T> ResultCollection { get; }
    }
}
