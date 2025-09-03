using BLL.Dto;

namespace BLL.Abstractions
{
    public interface IParticipantService
    {
        public Task<CallbackDto<ParticipantDto>> Get(Guid id);
        public Task<CallbackDto<bool>> Add(CreateParticipantDto dto);
        public Task<CallbackDto<bool>> Update(UpdateParticipantDto dto);
        public Task<CallbackDto<bool>> Delete(DeleteDto dto);
    }
}
