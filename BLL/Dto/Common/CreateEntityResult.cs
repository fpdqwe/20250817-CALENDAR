namespace BLL.Dto.Common
{
    public class CreateEntityResult
    {
        public bool IsSuccess { get; set; }
        public string? Issue { get; set; }
        public Guid? Id { get; set; }
    }
}
