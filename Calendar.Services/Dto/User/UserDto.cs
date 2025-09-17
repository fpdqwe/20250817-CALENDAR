using Calendar.Services.Abstractions;

namespace Calendar.Services.Dto.User
{
    public class UserDto : IDto<Calendar.Domain.Entities.User>
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public Calendar.Domain.Entities.User ToEntity()
        {
            return new Calendar.Domain.Entities.User
            {
                Login = Login,
                Password = Password
            };
        }
    }
}
