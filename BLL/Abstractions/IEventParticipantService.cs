using BLL.Dto;
using Domain.Entities;

namespace BLL.Abstractions
{
    public interface IEventParticipantService
    {
        public Task<CallbackDto<EventParticipant>> Get(Guid id);
        public Task<CallbackDto<bool>> Add(EventParticipant entity);
        public Task<CallbackDto<bool>> Update(EventParticipant entity);
        public Task<CallbackDto<bool>> Delete(EventParticipant entity);
    }
}
