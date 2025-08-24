using BLL.Abstractions;
using BLL.Dto;
using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository<Event> _repository;
        private readonly ILogger _logger;
        public EventService(IRepository<Event> repository,
            ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _repository = repository;
            _logger = logger;
            _logger.LogDebug($"New instance of {nameof(EventService)} was initialized");
        }
        public async Task<CallbackDto<Event>> Get(Guid id)
        {
            _logger.LogDebug($"Trying to get event id: {id}");
            var callback = new CallbackDto<Event>();
            var result = await _repository.Get(id);
            if (result == null)
            {
                var error = $"Failed to get event id: {id} from repository";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
            }
            else
            {
                callback.AddObject(result);
            }
            return callback;
        }
        public async Task<CallbackDto<bool>> Update(Event entity)
        {
            _logger.LogDebug($"Trying to update event id: {entity.Id}");
            var callback = new CallbackDto<bool>();
            if (entity == null)
            {
                callback.SetErrorMessage("Entity was null");
                return callback;
            }
            var result = await _repository.Update(entity);
            callback.AddObject(result);

            return callback;
        }
        public async Task<CallbackDto<bool>> Add(Event entity)
        {
            _logger.LogDebug($"Trying to create a new event, id: {entity.Id}");
            var callback = new CallbackDto<bool>();
            if (entity == null)
            {
                callback.SetErrorMessage("Entity was null");
                return callback;
            }
            var result = await _repository.Update(entity);
            callback.AddObject(result);
            return callback;
        }
        public async Task<CallbackDto<bool>> Delete(Event entity)
        {
            _logger.LogDebug($"Trying to delete event id: {entity.Id}");
            var callback = new CallbackDto<bool>();
            if (entity == null)
            {
                callback.SetErrorMessage("Entity was null");
                return callback;
            }
            var result = await _repository.Delete(entity);
            callback.AddObject(result);
            return callback;
        }
    }
}
