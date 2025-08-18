using BLL.Abstractions;
using Domain.Entities;

namespace BLL.Dto
{
    public class UserDto : IDto<User>
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public User ToEntity()
        {
            return new User { Login = Login, Password = Password };
        }
    }
}
