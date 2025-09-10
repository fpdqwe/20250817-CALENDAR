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
            _logger.LogDebug("Trying to get all event of user: {UserId}, by {Year} year",
                userId, year);
            var callback = new CallbackDto<List<EventDto>>();
            var result = await _repository.GetByUserId(userId, year);
            if (result.IsSuccess && result.Result != null && result.Result.Any())
            {
                var filtered = result.Result.Select(x => x.ToDto(year)).ToList();
                callback.AddObject(filtered);
            }
            else
            {
                var error = $"Failed to load events of user: {userId} from repository";
                _logger.LogInformation(error);
                callback.SetErrorMessage(error);
            }
            return callback;
        }
        public async Task<CallbackDto<EventDto>> Get(Guid id)
        {
            _logger.LogDebug("Trying to get event id: {Id}", id);
            var callback = new CallbackDto<EventDto>();
            var result = await _repository.Get(id);
            if (result.IsSuccess && result.Result != null)
            {
                callback.AddObject(result.Result.ToDto());
            }
            else
            {
                var error = $"Failed to get event id: {id} from repository";
                _logger.LogWarning(error);
                callback.SetErrorMessage(error);
            }
            return callback;
        }
        public async Task<CallbackDto<bool>> Update(UpdateEventDto dto)
        {
            _logger.LogDebug("Trying to update event id: {Id}", dto.Id);
            var callback = new CallbackDto<bool>();
            var result = await _repository.Update(dto.ToEntity());
            callback.AddObject(result.Result);

            return callback;
        }
        public async Task<CallbackDto<Guid>> Add(CreateEventDto dto)
        {
            _logger.LogDebug("Trying to create a new event: {Name}", dto.Name);

            var callback = new CallbackDto<Guid>();
            if (dto == null)
            {
                callback.SetErrorMessage("Invalid dto model");
                return callback;
            }
            var result = await _repository.Add(dto.ToEntity());
            if (result.IsSuccess && result.Result != Guid.Empty)
            {
                callback.AddObject(result.Result);
            }
            else
            {
                _logger.LogInformation("Failed to create new event");
                callback.SetErrorMessage("Failed to create new event");
                return callback;
            }
            return callback;
        }
        public async Task<CallbackDto<bool>> Delete(DeleteDto dto)
        {
            _logger.LogDebug($"Trying to delete event id: {dto.Id}");
            var callback = new CallbackDto<bool>();
            if (dto == null)
            {
                callback.SetErrorMessage("Invalid dto model");
                return callback;
            }
            var result = await _repository.Delete(dto.ToEvent());
            callback.AddObject(result.Result);
            return callback;
        }
    }
}
