using DataAccess.Abstractions;

namespace DataAccess.Results
{
    public class BoolResult : BaseDataAccessResult<bool>
    {
        public BoolResult(long duration) : base(duration) { }
        public BoolResult(Exception ex, long duration) : base(ex, duration) { }
        public BoolResult(bool isSuccess, bool result, string? exMessage, long duration = 0)
            : base(isSuccess, result, exMessage, duration) { }
        public BoolResult(bool result, long duration)
        {
            IsSuccess = true;
            Message = result ? FAIL : SUCCESS;
            Result = result;
            Duration = duration;
        }

    }
}
