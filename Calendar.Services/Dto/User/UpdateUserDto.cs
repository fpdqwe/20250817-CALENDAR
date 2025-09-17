using Calendar.Domain.Enums;
using Calendar.Services.Abstractions;

namespace Calendar.Services.Dto.User
{
    public class UpdateUserDto : IDto<Calendar.Domain.Entities.User>
    {
        public Guid Id { get; set; }
        public string? Login { get; set; } = null;
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Password { get; set; } = null;
        public ZodiacSign Zodiac { get; set; } = default;
        public DateTime DateOfBirth { get; set; } = default;

        public Calendar.Domain.Entities.User ToEntity()
        {
            return new Calendar.Domain.Entities.User
            {
                Id = Id,
                Login = Login,
                Name = Name,
                Lastname = Surname,
                Password = Password,
                Zodiac = Zodiac,
                DateOfBirth = DateOfBirth
            };
        }
    }
}
