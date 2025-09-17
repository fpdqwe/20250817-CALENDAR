using Calendar.Services.Dto.Common;
using Calendar.Services.Dto.Participant;

namespace Calendar.Services.Abstractions
{
    public interface IParticipantService
    {
        public Task<CallbackDto<ParticipantDto>> Get(Guid id);
        public Task<CallbackDto<Guid>> Add(CreateParticipantDto dto);
        public Task<CallbackDto<bool>> Update(UpdateParticipantDto dto);
        public Task<CallbackDto<bool>> Delete(DeleteDto dto);
    }
}
