using Domain.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class EventParticipant : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; } = "Owner";
        public int WarningTimeOffset { get; set; } = 0;
        public string? Color { get; set; }
        [ForeignKey(nameof(EventId))]
        public Event? Event { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
