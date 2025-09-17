using Calendar.Domain.Abstractions;

namespace Calendar.DataAccess.Abstractions
{
    public interface ICollectionResult<T> : IResult<T> where T : class, IEntity
    {
        public ICollection<T> ResultCollection { get; }
    }
}
