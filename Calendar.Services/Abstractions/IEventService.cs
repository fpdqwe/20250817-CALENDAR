using Calendar.Services.Dto.Common;
using Calendar.Services.Dto.Event;

namespace Calendar.Services.Abstractions
{
    public interface IEventService
    {
        Task<CallbackDto<List<EventDto>>> GetByUserId(Guid userId, int year);
        public Task<CallbackDto<EventDto>> Get(Guid id);
        public Task<CallbackDto<bool>> Update(UpdateEventDto dto);
        public Task<CallbackDto<Guid>> Add(CreateEventDto dto);
        public Task<CallbackDto<bool>> Delete(DeleteDto dto);
    }
}
