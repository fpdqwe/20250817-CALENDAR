using Domain.Abstractions;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(50)]
        public string Login { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Name { get; set; }
        [MaxLength(200)]
        public string? Surname { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
        public ZodiacSign Zodiac { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
        public DateTime DateOfBirth { get; set; }
    }
}
