using DataAccess.Abstractions;

namespace DataAccess.Results
{
    public class GuidResult : BaseDataAccessResult<Guid>
    {
        public GuidResult(long duration) : base(duration) { }
        public GuidResult(Exception ex, long duration) : base(ex, duration) { }
        public GuidResult(bool isSuccess, Guid result, string? exMessage, long duration = 0)
            : base(isSuccess, result, exMessage, duration) { }
        public GuidResult(Guid result, long duration) : base(result, duration)
        {
            IsSuccess = result != Guid.Empty;
            Message = IsSuccess ? SUCCESS : FAIL;
            Result = result;
            Duration = duration;
        }
    }
}
