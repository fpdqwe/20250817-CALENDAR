namespace BLL.Dto
{
    public class CreateParticipantDto
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; } = "participant";
        public int WarningTimeOffset { get; set; }
        public string Color { get; set; } = "9896ff";
    }
}
