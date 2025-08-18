using Domain.Abstractions;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Event : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Time { get; set; }
        public TimeSpan Duration { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(300)]
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string? Ico { get; set; }
        public IterationTime IterationTime { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
