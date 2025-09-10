using Domain.Enums;

namespace BLL.Dto
{
    public class UpdateEventDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime? Date { get; set; }
        public int? Duration { get; set; } // Duration in 15min intervals
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string? Ico { get; set; }
        public IterationTime? IterationTime { get; set; }
    }
}
