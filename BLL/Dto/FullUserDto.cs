using Domain.Enums;

namespace BLL.Dto
{
    public class FullUserDto
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public ZodiacSign Zodiac { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
