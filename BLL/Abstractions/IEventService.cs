using BLL.Dto;

namespace BLL.Abstractions
{
    public interface IEventService
    {
        Task<CallbackDto<List<EventDto>>> GetByUserId(Guid userId, int year);
        public Task<CallbackDto<EventDto>> Get(Guid id);
        public Task<CallbackDto<bool>> Update(UpdateEventDto dto);
        public Task<CallbackDto<bool>> Add(CreateEventDto dto);
        public Task<CallbackDto<bool>> Delete(DeleteDto dto);
    }
}
