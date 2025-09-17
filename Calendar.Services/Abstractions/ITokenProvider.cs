using Calendar.Domain.Entities;

namespace Calendar.Services.Abstractions
{
    public interface ITokenProvider
    {
        public string Generate(User user);
    }
}
