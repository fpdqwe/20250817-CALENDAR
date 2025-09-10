namespace BLL.Dto
{
    public class UpdateParticipantDto
    {
        public Guid Id { get; set; }
        public string? Role { get; set; }
        public int WarningTimeOffset { get; set; }
        public string? Color { get; set; }
    }
}
