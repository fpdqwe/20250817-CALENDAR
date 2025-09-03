namespace BLL.Dto
{
    public class ParticipantDto
    {
        public Guid Id { get; set; }
        public string? Role { get; set; }
        public int WarningTimeOffset { get; set; }
        public string? Color { get; set; }
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
    }
}
