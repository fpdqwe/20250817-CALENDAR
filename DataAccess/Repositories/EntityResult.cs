using DataAccess.Abstractions;
using Domain.Abstractions;

namespace DataAccess.Repositories
{
    public class EntityResult<T> : BaseDataAccessResult<T> where T : class, IEntity
    {

    }
}
