using BLL.Abstractions;
using BLL.Dto;
using BLL.Extensions;
using DataAccess.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository<Event> _repository;
        private readonly ILogger _logger;
        public EventService(IEventRepository<Event> repository,
            ILogger<EventService> logger)
        {
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _repository = repository;
            _logger = logger;
            _logger.LogDebug($"New instance of {nameof(EventService)} was initialized");
        }
        public async Task<CallbackDto<List<EventDto>>> GetByUserId(Guid userId, int year)
        {
            _logger.LogDebug($"Trying to get all event of user: {userId}, by {year} year");
            var callback = new CallbackDto<List<EventDto>>();
            var rawData = await _repository.GetByUserId(userId, year);
            if (rawData == null)
            {
                var error = $"Failed to load events of user: {userId} from repository";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
            }
            else
            {
                var result = rawData.Select(x => x.ToDto(year)).ToList();
                callback.AddObject(result);
            }
            return callback;
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
            var result = await _repository.Add(entity);
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
