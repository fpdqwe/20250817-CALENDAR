using DataAccess.Abstractions;

namespace DataAccess.Results
{
    public class EntityResult<T> : BaseDataAccessResult<T>
    {
        public EntityResult(long duration) : base(duration) { }
        public EntityResult(T? result, long duration) : base(result, duration) { }
        public EntityResult(Exception ex, long duration) : base(ex, duration) { }
        public EntityResult(bool isSuccess, T? result, string? exMessage, long duration = 0)
            : base(isSuccess, result, exMessage, duration) { }
    }
}
