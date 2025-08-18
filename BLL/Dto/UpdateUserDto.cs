using BLL.Abstractions;
using Domain.Entities;
using Domain.Enums;

namespace BLL.Dto
{
    public class UpdateUserDto : IDto<User>
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Password { get; set; } = string.Empty;
        public ZodiacSign Zodiac { get; set; }
        public DateTime DateOfBirth { get; set; }

        public User ToEntity()
        {
            return new User
            {
                Id = Id,
                Login = Login,
                Name = Name,
                Surname = Surname,
                Password = Password,
                Zodiac = Zodiac,
                DateOfBirth = DateOfBirth
            };
        }
    }
}
