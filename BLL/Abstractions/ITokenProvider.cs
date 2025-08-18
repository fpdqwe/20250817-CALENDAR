using Domain.Entities;

namespace BLL.Abstractions
{
    public interface ITokenProvider
    {
        public string Generate(User user);
    }
}
