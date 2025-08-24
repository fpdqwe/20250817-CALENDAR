using BLL.Dto;
using Domain.Entities;

namespace BLL.Abstractions
{
    public interface IEventService
    {
        public Task<CallbackDto<Event>> Get(Guid id);
        public Task<CallbackDto<bool>> Update(Event entity);
        public Task<CallbackDto<bool>> Add(Event entity);
        public Task<CallbackDto<bool>> Delete(Event entity);
    }
}
